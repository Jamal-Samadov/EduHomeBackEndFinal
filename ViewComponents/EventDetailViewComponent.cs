using EduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.ViewComponents
{
    public class EventDetailViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public EventDetailViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var eventDetails = await _dbContext.Events.ToListAsync();
            return View(eventDetails);
        }
    }
}
