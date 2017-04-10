using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    [Table("Exams")]
    public class Exam
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public float Duration { get; set; }

        [Required]
        public int From { get; set; }

        [Column("Cr_Id"), ForeignKey("Course")]
        public string CourseId { get; set; }

        public virtual Course Course { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}