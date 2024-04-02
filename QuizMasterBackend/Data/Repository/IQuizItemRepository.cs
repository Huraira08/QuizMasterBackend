using QuizMasterBackend.Models;

namespace QuizMasterBackend.Data.Repository
{
    public interface IQuizItemRepository
    {
        Task<List<QuizItem>> GetAllQuizItems();
        Task<QuizItem> GetQuizItem(int id);
        Task AddQuizItem(QuizItem quizItem);
        Task UpdateQuizItem(QuizItem newQuizItem);
        Task DeleteQuizItem(int id);

        Task<List<ResultDTO>> GetResults(string id);
        Task AddResult(ResultDTO resultDTO, string id);
        Task<List<ResultDTO>> GetTop10Results();

    }
}
