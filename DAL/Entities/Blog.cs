namespace EduHome.DAL.Entities
{
    public class Blog : Entity
    {
        public string ImageUrl { get; set; }
        public string BlogName { get; set; }
        public string BlogDescription { get; set; }
        public string Reply { get; set; }
    }
}
