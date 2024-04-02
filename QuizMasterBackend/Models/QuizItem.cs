using System.ComponentModel.DataAnnotations;

namespace QuizMasterBackend.Models
{
    public class QuizItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        public List<string> Answers { get; set; }
        [Required]
        [Range(0,3)]
        public int CorrectAnswerIndex { get; set; }
    }
}
