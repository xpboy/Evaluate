using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Evaluate.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //public int? PersonId { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string SecondLastName { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Evaluate.Models.Career> Careers { get; set; }
        public System.Data.Entity.DbSet<Evaluate.Models.Subject> Subjects { get; set; }
        public System.Data.Entity.DbSet<Evaluate.Models.Question> Questions { get; set; }
        public System.Data.Entity.DbSet<Evaluate.Models.Answer> Answers { get; set; }
        public System.Data.Entity.DbSet<Evaluate.Models.Evaluation> Evaluations { get; set; }
        public System.Data.Entity.DbSet<Evaluate.Models.EvaluationQuestion> EvaluationQuestions { get; set; }
        public System.Data.Entity.DbSet<Evaluate.Models.EvaluationDetail> EvaluationDetails { get; set; }
        public System.Data.Entity.DbSet<Evaluate.Models.StudentCareer> StudentCareers { get; set; }
        public System.Data.Entity.DbSet<Evaluate.Models.Student> Students { get; set; }

        public System.Data.Entity.DbSet<Evaluate.Models.Person> People { get; set; }

        public System.Data.Entity.DbSet<Evaluate.Models.AspNetUser> AspNetUser { get; set; }

        public System.Data.Entity.DbSet<Evaluate.Models.Plan> Plans { get; set; }

        public System.Data.Entity.DbSet<Evaluate.Models.Link> Links { get; set; }

        //public System.Data.Entity.DbSet<Evaluate.Models.ApplicationUser> ApplicationUsers { get; set; }
        //public System.Data.Entity.DbSet<Evaluate.Models.AspNetUserRole> AspNetUserRole { get; set; }
        //public System.Data.Entity.DbSet<Evaluate.Models.AspNetRole> AspNetRole { get; set; }
    }
}