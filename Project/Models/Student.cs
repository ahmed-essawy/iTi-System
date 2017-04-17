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

        public int Degrees { get; private set; }
        public int Absences { get; private set; }

        public void IsLate()
        {
            if (1 == 0)// Without Permission
            {
                Absences -= 25;
            }
            else
                switch (Absences)
                {
                    case 1:
                    case 2:
                    case 3:
                        Degrees -= 5;
                        break;

                    case 4:
                    case 5:
                    case 6:
                        Degrees -= 10;
                        break;

                    default:
                        Degrees -= 25;
                        break;
                }
            ++Absences;
        }

        [NotMapped]
        public virtual IEnumerable<Permission> Permissions
        {
            get => PermissionsCollection != null ? PermissionsCollection.Where(p => p.StudentId == Id) : new List<Permission>();
            set => PermissionsCollection = value.ToList();
        }

        [NotMapped]
        public virtual ICollection<Attendance> Attendances { get; set; }

        [NotMapped]
        public virtual ICollection<Permission> PermissionsCollection { get; set; }
    }
}