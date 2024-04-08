using QuizMasterBackend.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuizMasterBackend.Models
{
    public class Result
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public DateTime AttemptedDate { get; set; }
        [Required]
        public int Score { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

    }
}
