using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    [Table("Questions")]
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public QuestionType Header { get; set; }

        [Required, StringLength(500)]
        public string Body { get; set; }

        [Required, Column("Correct"), StringLength(500)]
        public string CorrectAnswer { get; set; }

        [Column("Cr_Id"), ForeignKey("Course")]
        public string CourseId { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual Course Course { get; set; }
    }

    [Table("Choices")]
    public class Answer
    {
        [Key, Column("Qs_Id", Order = 0), ForeignKey("Question")]
        public int QuestionId { get; set; }

        [Key, Column(Order = 1)]
        public string Choice { get; set; }

        public virtual Question Question { get; set; }
    }

    public enum QuestionType
    {
        MCQ,
        TF
    }
}