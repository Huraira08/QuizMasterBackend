namespace QuizMasterBackend.Models
{
    public class ResultDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime AttemptedDate { get; set; }
        public int Score { get; set; }
    }
}
