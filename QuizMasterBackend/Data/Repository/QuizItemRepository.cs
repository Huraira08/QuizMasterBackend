using Microsoft.EntityFrameworkCore;
using QuizMasterBackend.Models;

namespace QuizMasterBackend.Data.Repository
{
    public class QuizItemRepository : IQuizItemRepository
    {
        private readonly ApplicationDbContext _db;

        public QuizItemRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<QuizItem>> GetAllQuizItems()
        {
            return await _db.QuizItems.ToListAsync();
        }

        public async Task<QuizItem> GetQuizItem(int id)
        {
            QuizItem? quizItem = await _db.QuizItems.FirstOrDefaultAsync(qItem=>qItem.Id == id);
            if(quizItem == null)
            {
                throw new Exception("Item not found");
            }
            return quizItem;

        }

        public async Task AddQuizItem(QuizItem quizItem)
        {
            QuizItem? prevItem = await _db.QuizItems.FirstOrDefaultAsync(qItem=> qItem.Id == quizItem.Id);
            if(prevItem != null)
            {
                throw new Exception("Item already exists");
            }
            await _db.QuizItems.AddAsync(quizItem);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateQuizItem(QuizItem newQuizItem)
        {
            QuizItem oldQuizItem = await GetQuizItem(newQuizItem.Id);
            oldQuizItem.Question = newQuizItem.Question;
            oldQuizItem.Answers = newQuizItem.Answers;
            oldQuizItem.CorrectAnswerIndex = newQuizItem.CorrectAnswerIndex;
            await _db.SaveChangesAsync();
        }

        public async Task DeleteQuizItem(int id)
        {
            QuizItem quizItem = await GetQuizItem(id);
            _db.QuizItems.Remove(quizItem);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ResultDTO>> GetResults(string id)
        {
            return await _db.Results.Where(r => r.UserId == id).Select(r=>new ResultDTO()
            {
                Id = r.Id,
                AttemptedDate = r.AttemptedDate,
                Score = r.Score,
            })
                .ToListAsync();
        }

        public async Task AddResult(ResultDTO resultDTO, string id)
        {
            ApplicationUser? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(user == null)
            {
                throw new Exception("User does not exist");
            }

            Result result = new Result()
            {
                AttemptedDate = resultDTO.AttemptedDate,
                Score = resultDTO.Score,
                UserId = id,
            };

            await _db.Results.AddAsync(result);
            await _db.SaveChangesAsync();
        }

        public async Task<List<ResultDTO>> GetTop10Results()
        {
            return await _db.Results.OrderByDescending(result => result.Score)
                .Take(10)
                .Include(result => result.User)
                .Select(result => new ResultDTO()
                {
                    Id = result.Id,
                    AttemptedDate = result.AttemptedDate,
                    Score = result.Score,
                    Name = result.User.UserName
                }).ToListAsync();
        }
    }
}
