using EduHome.Areas.admin.Models;
using EduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.admin.ViewComponents
{
    public class ContactMessageViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public ContactMessageViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var messages = await _dbContext.ContactMessages.ToListAsync();
            
            var isAllRead = messages.All(x=>x.IsRead);

            return View(new ContactMessageViewModel
            {
                ContactMessages = messages,
                IsAllRead = isAllRead,
            });
        }
    }
}
