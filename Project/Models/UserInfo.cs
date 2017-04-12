using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class UserInfo : ApplicationUser
    {
        [Required, StringLength(25), Display(Name = "First Name")]
        public string Fname { get; set; }

        [Required, StringLength(25), Display(Name = "Last Name")]
        public string Lname { get; set; }

        [NotMapped]
        public string Name { get { return Fname + " " + Lname; } }

        [Required, NotMapped, DataType(DataType.Password), Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [NotMapped, DataType(DataType.Password), Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Birthdate")]
        public DateTime? Bdate { get; set; }

        [Display(Name = "Is Married")]
        public bool? IsMarried { get; set; }

        [Column("Dp_Id"), ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
    }
}