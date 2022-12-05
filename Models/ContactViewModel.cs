using System.ComponentModel.DataAnnotations;

namespace EduHome.Models
{
    public class ContactViewModel
    {
        public ContactMessageViewModel ContactMessage { get; set; } = new();
    }

    public class ContactMessageViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string? Subject { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
