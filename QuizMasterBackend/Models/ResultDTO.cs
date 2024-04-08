namespace QuizMasterBackend.Models
{
    public class ResultDTO
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public DateTime AttemptedDate { get; set; }
        public int Score { get; set; }
        public string UserId { get; set; }
    }
}
