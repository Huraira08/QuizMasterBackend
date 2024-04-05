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
            List<QuizItem> quizItems = await _db.QuizItems.ToListAsync();
            return quizItems;
        }

        public async Task<QuizItem?> Get(int id)
        {
            QuizItem? quizItem = await _db.QuizItems.FirstOrDefaultAsync(qItem=>qItem.Id == id);
            return quizItem;
        }

        public async Task<int> AddOrUpdate(QuizItem quizItem)
        {
            if(quizItem.Id == 0)
            {
                await _db.QuizItems.AddAsync(quizItem);
            }
            else
            {
                QuizItem? prevItem = await Get(quizItem.Id);
                if (prevItem == null)
                {
                    return -1;
                }
                prevItem.Question = quizItem.Question;
                prevItem.Answers = quizItem.Answers;
                prevItem.CorrectAnswerIndex = quizItem.CorrectAnswerIndex;
            }
            
            int rowsAffected = await _db.SaveChangesAsync();
            return rowsAffected;
        }

        //public async Task<int> UpdateQuizItem(QuizItem newQuizItem)
        //{
        //    QuizItem? oldQuizItem = await Get(newQuizItem.Id);
        //    if(oldQuizItem == null)
        //    {
        //        return 0;
        //    }
        //    oldQuizItem.Question = newQuizItem.Question;
        //    oldQuizItem.Answers = newQuizItem.Answers;
        //    oldQuizItem.CorrectAnswerIndex = newQuizItem.CorrectAnswerIndex;
        //    return await _db.SaveChangesAsync();
        //}

        public async Task<int> DeleteQuizItem(int id)
        {
            QuizItem? quizItem = await Get(id);
            if(quizItem == null)
            {
                return 0;
            }
            _db.QuizItems.Remove(quizItem);
            return await _db.SaveChangesAsync();
        }
    }
}
