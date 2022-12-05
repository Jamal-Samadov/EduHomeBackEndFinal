namespace EduHome.Areas.admin.Models
{
    public class SpeakerUpdateModel
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Job { get; set; }
        public string Position { get; set; }
        public IFormFile? Image { get; set; }
    }
}
