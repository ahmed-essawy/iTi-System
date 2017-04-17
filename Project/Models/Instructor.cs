using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Project.Models
{
    [Table("Instructors")]
    public class Instructor : UserInfo
    {
        [Column("Grad_Year"), Range(1901, 2020), Display(Name = "Graduation Year")]
        public int? GraduationYear { get; set; }

        public Status Status { get; set; }

        public virtual IEnumerable<Qualification> Qualifications
        {
            get => QualificationsCollection != null ? QualificationsCollection.Where(x => x.InstructorId == Id) : new List<Qualification>();
            set => QualificationsCollection = value.ToList();
        }

        public virtual ICollection<Qualification> QualificationsCollection { get; set; }
    }

    public enum Status { Internal, External }

    [Table("Qualifications")]
    public class Qualification
    {
        [Key, Column("In_Id", Order = 1), ForeignKey("Instructor")]
        public string InstructorId { get; set; }

        [Key, Column(Order = 0)]
        public string Name { get; set; }

        public virtual Instructor Instructor { get; set; }
    }
}