using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Project.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Fname { get; set; }

        [Required]
        [StringLength(25)]
        public string Lname { get; set; }

        [Required]
        public DateTime BDate { get; set; }

        public bool IsMarried { get; set; }

        [ForeignKey("Departments")]
        public int? DpId { get; set; }

        public virtual IdentityUser IdentityUser { get; set; }
        public virtual Department Departments { get; set; }
    }
}