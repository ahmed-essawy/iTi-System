using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Exam
    {   [Key]
        public int Id { get; set; }
        [Required]
        public float Duration { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public int From { get; set; }
        [Required]
        public int To { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Course> Course { get; set; }
    }
}