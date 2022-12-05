namespace EduHome.Areas.admin.Models
{
    public class BlogUpdateModel
    {
        public string? ImageUrl { get; set; }
        public string BlogName { get; set; }
        public string BlogDescription { get; set; }
        public string Reply { get; set; }
        public IFormFile Image { get; set; }
    }
}
