using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizMasterBackend.Data.Repository;
using QuizMasterBackend.Models;
using QuizMasterBackend.Utility;

namespace QuizMasterBackend.Controllers
{
    [Authorize(Roles = UserRoles.User)]
    [Route("api/[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private readonly IResultRepository _resultRepository;

        public ResultController(IResultRepository resultRepository)
        {
            _resultRepository = resultRepository;
        }

        // Results
        [HttpGet("{id}")]
        public async Task<List<ResultDTO>> GetResults(string id)
        {
            List<ResultDTO> results = await _resultRepository.GetResults(id);
            return results;
        }

        [HttpPost()]
        public async Task<IActionResult> AddResult(ResultDTO resultDTO)
        {
            ResultDTO? newResult = await _resultRepository.AddResult(resultDTO);
            if (newResult != null)
            {
                return CreatedAtAction(nameof(GetResults), new { id = newResult.Id }, newResult);
            }
            else
            {
                return BadRequest("Invalid user id");
            }
        }

        [HttpGet("top10")]
        public async Task<List<ResultDTO>> GetTop10()
        {
            List<ResultDTO> results = await _resultRepository.GetTop10Results();
            return results;
        }
    }
}
