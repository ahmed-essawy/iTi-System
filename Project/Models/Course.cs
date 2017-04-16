using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    [Table("Courses")]
    public class Course
    {
        [Key, Column("Code")]
        public string Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, Column("Lec_Duration"), Range(1, 20), Display(Name = "Lecture Duration")]
        public int LectureDuration { get; set; }

        [Required, Column("Lab_Duration"), Range(1, 20), Display(Name = "Lab Duration")]
        public int LabDuration { get; set; }

        public virtual ICollection<Department> Departments { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}