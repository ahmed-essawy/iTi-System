using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Project.Models
{
    public enum Status { Internal, External }

    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        public DateTime Bdate { get; set; }
        public bool IsMarried { get; set; }
        public List<string> Qualifications { get; set; }
        public int GraduationsYear { get; set; }
        public Status Status { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        [InverseProperty("Instructors")]
        public virtual Department Department { get; set; }
        [InverseProperty("Instructor")]
        public virtual Department ManageDepartment { get; set; }
        public virtual Department Departments { get; set; }
        [ForeignKey("Departments")]
        public int? DpId { get; set; }

        //[InverseProperty("Instructors")]
        //public virtual Department Departments { get; set; }

        //[InverseProperty("InId")]
        //public virtual Department ManageDept { get; set; }

        public virtual IdentityUser IdentityUser { get; set; }
    }
}