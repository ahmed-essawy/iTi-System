using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project.Models
{
    public class StInCr
    {
        [Key,Column(Order =0),ForeignKey("Student")]
        public int StId { get; set; }
        [Key, Column(Order = 1), ForeignKey("Instructor")]

        public int InIs { get; set; }
        [Key, Column(Order = 2), ForeignKey("Course")]

        public string CrCode { get; set; }
        public string Eval { get; set; }
        public virtual Student Student { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual Course Course { get; set; }

    }
}