using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class StEx
    {
        [ForeignKey("Exam")]
        public int ExId { get; set; }
        [ForeignKey("Students")]
        public int StId { get; set; }
        [Required]
        public float Grade { get; set; }
        public virtual Exam Exam { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}