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

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Column("Lec_Duration")]
        public int LectureDuration { get; set; }

        [Column("Lab_Duration")]
        public int LabDuration { get; set; }

        public virtual ICollection<Department> Departments { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}