using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Project.Models
{
    public class Department
    {
        public Department()
        {
            Capacity = 25;
        }

        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required, Column("Manager_Id"), ForeignKey("Manager")]
        public string ManagerId { get; set; }

        public virtual Instructor Manager { get; set; }

        [NotMapped]
        public virtual List<Student> Students { get { return new List<Student>().Where(x => x.DepartmentId == Id).ToList(); } }

        public bool IsFree()
        {
            return Students.Count() < Capacity ? true : false;
        }

        public virtual ICollection<Course> Courses { get; set; }
    }
}