using Microsoft.AspNetCore.Mvc.Rendering;

namespace EduHome.Areas.admin.Models
{ 
    public class EventCreateModel
    {
        public string EventName { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Reply { get; set; }
        public IFormFile Image { get; set; }
        public List<SelectListItem>? Speakers { get; set; }
        public List<int> SpeakerIds { get; set; }

    }

}
