using QuizMasterBackend.Models;

namespace QuizMasterBackend.Data.Repository
{
    public interface IResultRepository
    {
        Task<List<ResultDTO>> GetResults(string id);
        Task<ResultDTO?> AddResult(ResultDTO resultDTO);
        Task<List<ResultDTO>> GetTop10Results();
    }
}
