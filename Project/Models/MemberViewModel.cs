namespace Project.Models
{
    public class MemberViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Name
        {
            get { return FirstName + " " + LastName; }
        }

        public bool IsMember { get; set; }
    }
}