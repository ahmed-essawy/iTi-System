using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class UserInfo : ApplicationUser
    {
        [Required, StringLength(25)]
        public string Fname { get; set; }

        [Required, StringLength(25)]
        public string Lname { get; set; }

        public DateTime? Bdate { get; set; }
        public bool? IsMarried { get; set; }

        [Column("Dp_Id"), ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
    }
}