using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    [Table("In_St_Cr")]
    public class InstructorStudentCourse
    {
        [Key, Column(Order = 0), ForeignKey("Instructor")]
        public string InstructorId { get; set; }

        [Key, Column(Order = 1), ForeignKey("Student")]
        public string StudentId { get; set; }

        [Key, Column(Order = 2), ForeignKey("Course")]
        public string CourseId { get; set; }

        [Column("In_Eval")]
        public int? InstructorEvaluation { get; set; }

        [Display(Name = "Lab Grade"), Column("Lab_Grade"), Range(0, 50)]
        public int? LabGrade { get; set; }

        [Display(Name = "Exam Grade"), Column("Exam_Grade"), Range(0, 100)]
        public int? ExamGrade { get; set; }

        public virtual Instructor Instructor { get; set; }
        public virtual Student Student { get; set; }
        public virtual Course Course { get; set; }
    }
}