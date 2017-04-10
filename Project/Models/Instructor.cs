using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Project.Models
{
    [Table("Instructors")]
    public class Instructor : UserInfo
    {
        [Column("Grad_Year"), Range(1901, 2017)]
        public int? GraduationYear { get; set; }

        public Status Status { get; set; }

        public virtual List<Qualification> Qualifications { get { return new List<Qualification>().Where(x => x.InstructorId == Id).ToList(); } }
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