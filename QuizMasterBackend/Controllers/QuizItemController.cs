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
            return await _quizItemRepository.Get();
        }

        // GET api/<QuizItemController>/5
        [HttpGet("{id}")]
        public async Task<QuizItem> Get(int id)
        {
            return await _quizItemRepository.Get(id);
        }

        // POST api/<QuizItemController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] QuizItem quizItem)
        {
            quizItem = await _quizItemRepository.AddQuizItem(quizItem);
            return CreatedAtAction(nameof(Get), new { id = quizItem.Id }, quizItem);
        }

        // PUT api/<QuizItemController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] QuizItem newQuizItem)
        {
            try
            {
                int rowsAffected = await _quizItemRepository.UpdateQuizItem(newQuizItem);
                return new JsonResult(new
                {
                    rowsAffected
                });
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
                int rowsAffected = await _quizItemRepository.DeleteQuizItem(id);
                return new JsonResult( new {
                    rowsAffected
                });
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
