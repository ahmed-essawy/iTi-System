using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    [Table("Students")]
    public class Student : UserInfo
    {
        //public virtual ICollection<Attendance> Attendances { get; set; }
        //public virtual ICollection<Instructor> Instructors { get; set; }
        //public virtual ICollection<Course> Courses { get; set; }
    }
}