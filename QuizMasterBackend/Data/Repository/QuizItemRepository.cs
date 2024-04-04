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

        public async Task<List<QuizItem>> Get()
        {
            return await _db.QuizItems.ToListAsync();
        }

        public async Task<QuizItem> Get(int id)
        {
            QuizItem? quizItem = await _db.QuizItems.FirstOrDefaultAsync(qItem=>qItem.Id == id);
            if(quizItem == null)
            {
                throw new Exception("Item not found");
            }
            return quizItem;

        }

        public async Task<QuizItem?> AddQuizItem(QuizItem quizItem)
        {
            QuizItem? prevItem = await _db.QuizItems.FirstOrDefaultAsync(qItem=> qItem.Id == quizItem.Id);
            if(prevItem != null)
            {
                //throw new Exception("Item already exists");
                return null;
            }
            await _db.QuizItems.AddAsync(quizItem);
            int rowsAffected = await _db.SaveChangesAsync();
            return quizItem;
        }

        public async Task<int> UpdateQuizItem(QuizItem newQuizItem)
        {
            QuizItem oldQuizItem = await Get(newQuizItem.Id);
            oldQuizItem.Question = newQuizItem.Question;
            oldQuizItem.Answers = newQuizItem.Answers;
            oldQuizItem.CorrectAnswerIndex = newQuizItem.CorrectAnswerIndex;
            return await _db.SaveChangesAsync();
        }

        public async Task<int> DeleteQuizItem(int id)
        {
            QuizItem quizItem = await Get(id);
            _db.QuizItems.Remove(quizItem);
            return await _db.SaveChangesAsync();
        }
    }
}
