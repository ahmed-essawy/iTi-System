using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Project.Models
{
    [Table("Students")]
    public class Student : UserInfo
    {
        public Student()
        {
            Degrees = 600;
            Absences = 0;
        }

        public int Degrees { get; set; }
        public int Absences { get; set; }

        [NotMapped]
        public virtual IEnumerable<Permission> Permissions
        {
            get { return PermissionsCollection != null ? PermissionsCollection.Where(p => p.StudentId == Id) : new List<Permission>(); }
            set { PermissionsCollection = value.ToList(); }
        }

        [NotMapped]
        public virtual ICollection<Attendance> Attendances { get; set; }

        [NotMapped]
        public virtual ICollection<Permission> PermissionsCollection { get; set; }
    }
}