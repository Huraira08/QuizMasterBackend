using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QuizMasterBackend.Data;
using QuizMasterBackend.Models;
using QuizMasterBackend.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizMasterBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return BadRequest("User already exists");
            }
            var newUser = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Name
            };
            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new {
                    Status = "Error",
                    Message = "Could not create new user",
                });
            }

            if (!_roleManager.RoleExistsAsync(UserRoles.User).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }
            if (!_roleManager.RoleExistsAsync(UserRoles.Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            if (_roleManager.RoleExistsAsync(UserRoles.User).GetAwaiter().GetResult())
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            return Ok(new {Status = "Succeeded", Message = "User Created Successfully"});
        }

        [HttpPost("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return BadRequest("Already exists");
            }
            var newUser = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Name
            };
            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Status = "Error",
                    Message = "Could not create new user",
                });
            }
            if (!_roleManager.RoleExistsAsync(UserRoles.Admin).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }
            if (!_roleManager.RoleExistsAsync(UserRoles.User).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            }

            if (_roleManager.RoleExistsAsync(UserRoles.User).GetAwaiter().GetResult())
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            }
            if (_roleManager.RoleExistsAsync(UserRoles.Admin).GetAwaiter().GetResult())
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);
            }

            return Ok(new { Status = "Succeeded", Message = "User created successfully" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(model.Email);
                bool isPasswordValid = await _userManager.CheckPasswordAsync(user!, model.Password);
                if(user != null && isPasswordValid)
                {
                    var roles = await _userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName!),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    foreach(var userRole in roles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(4),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo,
                        user = new UserDTO
                        {
                            Id = user.Id,
                            Name = user.UserName,
                            Email = user.Email!
                        }
                    });
                }
                return Unauthorized();
            }catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize(Roles = $"{UserRoles.User},{UserRoles.Admin}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return BadRequest("User not found");
            }

            return Ok(user);
        }

        [HttpGet("validate-token")]
        public IActionResult ValidateToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if(token == null)
            {
                return Unauthorized();
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            if(jwtToken == null)
            {
                return new JsonResult(new { isExpired = true }); ;
            }
            else
            {
                var expiryTime = jwtToken.ValidTo;
                return new JsonResult(new {
                    isExpired = (expiryTime < DateTime.UtcNow) 
                });
            }
        }
    }
}
