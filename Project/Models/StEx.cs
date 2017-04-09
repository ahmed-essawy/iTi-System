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
        [Key,Column(Order =0),ForeignKey("Exam")]
        public int ExId { get; set; }

        [Key, Column(Order = 1),ForeignKey("Student")]
        public int StId { get; set; }

        [Required]
        public float Grade { get; set; }

        public virtual Student Student { get; set; }
        public virtual Exam Exam { get; set; }
    }
}