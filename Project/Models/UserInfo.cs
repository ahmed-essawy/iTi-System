using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Project.Models
{
    public class UserInfo : ApplicationUser
    {
        [Required(AllowEmptyStrings = false), StringLength(25), Display(Name = "First Name"), Column("Fname")]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false), StringLength(25), Display(Name = "Last Name"), Column("Lname")]
        public string LastName { get; set; }

        [NotMapped]
        public string Name { get { return FirstName + " " + LastName; } }

        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        [Remote("IsEmailExist", "Main", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "The {0} already exists. Please, enter a different {0}.")]
        public override string Email { get; set; }

        //[Required]
        [Remote("IsUserNameExist", "Main", AdditionalFields = "Id", HttpMethod = "POST", ErrorMessage = "The {0} already exists. Please, enter a different {0}.")]
        public override string UserName { get; set; }

        [Required, NotMapped, DataType(DataType.Password), Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [NotMapped, DataType(DataType.Password), Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Birthdate"), Column("Bdate"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Birthdate { get; set; }

        [Display(Name = "Is Married")]
        public bool? IsMarried { get; set; }

        [Column("Dp_Id"), ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
    }
}