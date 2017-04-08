using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class InDpCr
    {
        [Key]
        public int DpId { get; set; }
        [Key]
        public int InId { get; set; }
        [Key]
        public string CrCode { get; set; }
        public virtual Department Department { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual Course Course { get; set; }
    }
}