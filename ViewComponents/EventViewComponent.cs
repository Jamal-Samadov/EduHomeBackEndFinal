using EduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.ViewComponents
{
    public class EventViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public EventViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var events = await _dbContext.Events.ToListAsync();
            return View(events);
        }
    }
}
