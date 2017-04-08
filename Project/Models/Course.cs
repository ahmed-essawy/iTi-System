using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Course
    {
        [Key]
        [Required]
        public string Code { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int LectDuration { get; set; }
        public int LabDuration { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual  ICollection<Instructor> Instructors { get; set; }
    }
}