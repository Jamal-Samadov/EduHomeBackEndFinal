using System.ComponentModel.DataAnnotations;

namespace EduHome.Areas.admin.Models
{
    public class TeacherUpdateModel
    {
        public string TeacherFullName { get; set; }
        public string Position { get; set; }
        public string About { get; set; }
        public string Degree { get; set; }
        public string Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        [EmailAddress]
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Skype { get; set; }
        public byte Language { get; set; }
        public byte TeamLeader { get; set; }
        public byte Development { get; set; }
        public byte Design { get; set; }
        public byte Innovation { get; set; }
        public byte Communication { get; set; }
        public string ImageUrl { get; set; } = String.Empty;

        public IFormFile Image { get; set; }
    }
}
