namespace EduHome.Areas.admin.Models
{
    public class SliderCreateMultipleModel
    {
        public string? Headtitle { get; set; }
        public string? Subtitle { get; set; }
        public IFormFile[] Images { get; set; } 
    }
}
