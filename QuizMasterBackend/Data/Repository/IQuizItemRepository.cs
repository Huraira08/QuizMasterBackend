using QuizMasterBackend.Models;

namespace QuizMasterBackend.Data.Repository
{
    public interface IQuizItemRepository
    {
        Task<List<QuizItem>> Get();
        Task<QuizItem> Get(int id);
        Task<QuizItem> AddQuizItem(QuizItem quizItem);
        Task<int> UpdateQuizItem(QuizItem newQuizItem);
        Task<int> DeleteQuizItem(int id);
    }
}
