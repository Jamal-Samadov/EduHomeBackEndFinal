using EduHome.DAL.Entities;

namespace EduHome.ViewModels
{
    public class EventViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Tags> Tags { get; set; }
        public List<Speaker> Speakers { get; set; }
        public Event Event { get; set; }
    }
}
