using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

//(Date , ArrivalTime , leavingTime
namespace Project.Models
{
    public class Attendance
    {
        [Key, Column(Order = 0)]
        public DateTime Date { set; get; }

        [Key, Column(Order = 1), ForeignKey("Student")]
        public int StId { get; set; }

        [Required]
        public DateTime ArrivalTime { set; get; }

        [Required]
        public DateTime LeavingTime { set; get; }
        public virtual Student Student { get; set; }

        // public virtual ICollection<Student> Students { get; set; }
    }
}