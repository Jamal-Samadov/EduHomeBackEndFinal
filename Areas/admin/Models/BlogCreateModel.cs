namespace EduHome.Areas.admin.Models
{
    public class BlogCreateModel
    {
        public string? ImageUrl { get; set; }
        public string BlogName { get; set; }
        public string BlogDescription { get; set; }
        public string Reply { get; set; }
        public IFormFile Image { get; set; }
    }
}
