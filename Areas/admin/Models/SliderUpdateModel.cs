namespace EduHome.Areas.admin.Models
{
    public class SliderUpdateModel
    {
        public string Headtitle { get; set; }

        public string Subtitle { get; set; }
        public string? ImageUrl { get; set; }

        public IFormFile Image { get; set; }
    }
}
