namespace EduHome.DAL.Entities
{
    public class Event : Entity
    {
        public string EventName { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Reply { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<EventSpeaker> EventSpeakers { get; set; }

    }
}
