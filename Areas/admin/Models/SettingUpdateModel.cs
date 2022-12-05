namespace EduHome.Areas.admin.Models
{
    public class SettingUpdateModel
    {
        public string? Key { get; set; }

        public string? Value { get; set; }
        public string ImageName { get; set; } = String.Empty;

        public IFormFile? Image { get; set; }
    }
} 
