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
        private readonly IQuizItemRepository _quizItemRepository;

        public ResultController(IQuizItemRepository quizItemRepository)
        {
            _quizItemRepository = quizItemRepository;
        }

        // Results
        [HttpGet("{id}")]
        public async Task<IActionResult> GetResults(string id)
        {
            try
            {
                List<ResultDTO> results = await _quizItemRepository.GetResults(id);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddResult(ResultDTO resultDTO, string id)
        {
            try
            {
                await _quizItemRepository.AddResult(resultDTO, id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
