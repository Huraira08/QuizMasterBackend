using Microsoft.AspNetCore.Identity;
using QuizMasterBackend.Models;

namespace QuizMasterBackend.Data
{
    public class ApplicationUser: IdentityUser
    {
        public List<Result> Results { get; set; } = [];
    }
}
