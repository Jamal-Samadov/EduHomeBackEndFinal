namespace EduHome.DAL.Entities
{
    public class Speaker : Entity
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Job { get; set; }
        public string Position { get; set; }
        public ICollection<EventSpeaker> EventSpeakers { get; set; }
    }
}
