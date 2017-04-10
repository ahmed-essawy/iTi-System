using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    [Table("Attendances")]
    public class Attendance
    {
        [Key, Column(Order = 0)]
        public DateTime Date { set; get; }

        [Key, Column("St_Id", Order = 1), ForeignKey("Student")]
        public string StudentId { get; set; }

        [Column("Arrival")]
        public DateTime? ArrivalTime { set; get; }

        [Column("Leaving")]
        public DateTime? LeavingTime { set; get; }

        public virtual Student Student { get; set; }
    }
}