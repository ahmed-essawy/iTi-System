using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class InDpCr
    {
        [Key,Column(Order =0),ForeignKey("Department")]
        public int DpId { get; set; }
        [Key,Column(Order =1), ForeignKey("Instructor")]
        public int InId { get; set; }
        [Key,Column(Order =2), ForeignKey("Course")]
        public string CrCode { get; set; }
        public virtual Department Department { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual Course Course { get; set; }
    }
}