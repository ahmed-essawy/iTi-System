using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    [Table("Students")]
    public class Student : UserInfo
    {
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}