using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Project.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", false)
        {
        }

        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Qualification> Qualifications { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<StudentExam> StudentsExams { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<InstructorStudentCourse> InstructorStudentCourse { get; set; }
        public virtual DbSet<Attendance> Attendaces { get; set; }
        public virtual DbSet<Vacation> Vacations { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<DailyReport> DailyReports { get; set; }

        protected override void OnModelCreating(DbModelBuilder dbModelBuilder)
        {
            base.OnModelCreating(dbModelBuilder);
            dbModelBuilder.Entity<Student>().Property(s => s.DepartmentId).IsRequired();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}