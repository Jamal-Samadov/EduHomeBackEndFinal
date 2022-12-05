using Microsoft.Build.Framework;

namespace EduHome.Areas.admin.Models
{
    public class SliderCreateModel
    {
        public string Headtitle { get; set; }

        public string Subtitle { get; set; }

        public IFormFile Image { get; set; } 
    }
}
