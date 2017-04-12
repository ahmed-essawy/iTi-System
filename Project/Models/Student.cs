using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    [Table("Students")]
    public class Student : UserInfo
    {
        public Student()
        {
            Degrees = 600;
        }

        public int Degrees { get; private set; }

        public void Subtract(int Degrees)
        {
            this.Degrees -= Degrees;
        }

        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}