using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Header { get; set; }

        [Required]
        [StringLength(500)]
        public string Body { get; set; }

        [Required]
        [StringLength(500)]
        public List<string> Answers { get; set; }

        [Required]
        [StringLength(500)]
        public string CorrectAnswer { get; set; }

        [Required]
        [StringLength(50)]
        public string Subject { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }
    }
}