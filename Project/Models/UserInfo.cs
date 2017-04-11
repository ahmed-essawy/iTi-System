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

        [Required, NotMapped, DataType(DataType.Password), Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [NotMapped, DataType(DataType.Password), Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public DateTime? Bdate { get; set; }
        public bool? IsMarried { get; set; }

        [Column("Dp_Id"), ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
    }
}