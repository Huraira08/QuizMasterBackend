using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizMasterBackend.Data.Repository;
using QuizMasterBackend.Models;
using QuizMasterBackend.Utility;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuizMasterBackend.Controllers
{
    [Authorize(Roles = UserRoles.User)]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QuizItemController : ControllerBase
    {
        private readonly IQuizItemRepository _quizItemRepository;

        public QuizItemController(IQuizItemRepository quizItemRepository)
        {
            _quizItemRepository = quizItemRepository;
        }
        // GET: api/<QuizItemController>
        [HttpGet]
        public async Task<List<QuizItem>> Get()
        {
            return await _quizItemRepository.GetAllQuizItems();
        }

        // GET api/<QuizItemController>/5
        [HttpGet("{id}")]
        public async Task<QuizItem> Get(int id)
        {
            return await _quizItemRepository.GetQuizItem(id);
        }

        // POST api/<QuizItemController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QuizItem quizItem)
        {
            try
            {
                await _quizItemRepository.AddQuizItem(quizItem);
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<QuizItemController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] QuizItem newQuizItem)
        {
            try
            {
                await _quizItemRepository.UpdateQuizItem(newQuizItem);
                return Ok();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<QuizItemController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _quizItemRepository.DeleteQuizItem(id);
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("top10")]
        public async Task<IActionResult> GetTop10()
        {
            try
            {
                return Ok(await _quizItemRepository.GetTop10Results());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new {error= e.Message });
            }
        }
    }
}
