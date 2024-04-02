using System.ComponentModel.DataAnnotations;

namespace QuizMasterBackend.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "User name is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Passowrd is required")]
        public string Password { get; set; }
    }
}
