using EduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.ViewComponents
{
    public class LatestPostViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public LatestPostViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var latestPost = await _dbContext.Blogs.ToListAsync();
            return View(latestPost);
        }
    }
}
