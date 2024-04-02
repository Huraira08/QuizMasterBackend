using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizMasterBackend.Models;

namespace QuizMasterBackend.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<QuizItem> QuizItems { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Result>()
                .HasOne(r=>r.User)
                .WithMany(u=>u.Results)
                .HasForeignKey(r=>r.UserId)
                .IsRequired();
            builder.Entity<QuizItem>()
                .HasData(
                new QuizItem {
                    Id = 1,
                    Question = "An interface design application that runs in the browser with team-based collaborative design projects",
                    Answers = [ "Figma", "Adobe XD", "Invision", "Sketch"],
                    CorrectAnswerIndex = 0,
                },
                new QuizItem
                {
                    Id = 2,
                    Question = "Which of the following is not a primary color of light?",
                    Answers = ["Red", "Yellow", "Blue", "Green"],
                    CorrectAnswerIndex = 1,
                },
                new QuizItem
                {
                    Id = 3,
                    Question = "What is the chemical symbol for water?",
                    Answers = ["O2", "CO2", "NaCl", "H2O"],
                    CorrectAnswerIndex = 3,
                },
                new QuizItem
                {
                    Id = 4,
                    Question = "What is the value of π (pi) to two decimal places?",
                    Answers = ["3.18", "3.16", "3.14", "3.12"],
                    CorrectAnswerIndex = 2,
                }
                );
        }
    }
}
