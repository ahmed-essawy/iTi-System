using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    [Table("St_Ex")]
    public class StudentExam
    {
        [Key, Column("St_Id", Order = 0), ForeignKey("Student")]
        public string StudentId { get; set; }

        [Key, Column("Ex_Id", Order = 1), ForeignKey("Exam")]
        public int ExamId { get; set; }

        public float? Grade { get; set; }

        public virtual Student Student { get; set; }
        public virtual Exam Exam { get; set; }
    }
}