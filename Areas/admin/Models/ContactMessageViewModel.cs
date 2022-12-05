using EduHome.DAL.Entities;

namespace EduHome.Areas.admin.Models
{
    public class ContactMessageViewModel
    {
        public List<ContactMessage> ContactMessages { get; set; }
        public bool IsAllRead { get; set; }
    }
}
