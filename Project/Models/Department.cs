using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    [Table("Departments")]
    public class Department
    {
        public Department() => Capacity = 25;

        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required, Column("Manager_Id"), ForeignKey("Manager")]
        public string ManagerId { get; set; }

        public virtual Instructor Manager { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        [NotMapped]
        public virtual ICollection<Student> Students { get; set; }
    }
}