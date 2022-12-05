namespace EduHome.Areas.admin.Models
{
    public class SettingCreateModel
    {
        public string Key { get; set; }
        public string? Value { get; set; }
        public bool IsMain { get; set; }
        public IFormFile? Image { get; set; }

    }
}
