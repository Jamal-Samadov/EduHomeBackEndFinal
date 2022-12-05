using EduHome.DAL;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers
{
    public class EventsController : Controller
    {
        private readonly AppDbContext _dbContext;


        public EventsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var blogs = await _dbContext.Blogs.Where(x => !x.IsDeleted).ToListAsync();
            var tags = await _dbContext.Tags.Where(x => !x.IsDeleted).ToListAsync();
            var categories = await _dbContext.Categories.Where(x => !x.IsDeleted).ToListAsync();
            var blogViewModel = new BlogViewModel
            {
                Tags = tags,
                Categories = categories,
            };
            return View(blogViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return BadRequest();
            var events = await _dbContext.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (events is null) return NotFound();

            var tags = await _dbContext.Tags.Where(x => !x.IsDeleted).ToListAsync();
            var categories = await _dbContext.Categories.Where(x => !x.IsDeleted).ToListAsync();
            var speakers = await _dbContext.Speakers.Where(x => !x.IsDeleted).ToListAsync();
            var eventViewModel = new EventViewModel
            {
                Tags = tags,
                Event = events,
                Categories = categories,
                Speakers = speakers,
            };
            return View(eventViewModel);
        }
    }
}
