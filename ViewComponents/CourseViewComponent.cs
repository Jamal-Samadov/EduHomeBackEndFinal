using EduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.ViewComponents
{
    public class CourseViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public CourseViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var course = await _dbContext.Courses.ToListAsync();
            return View(course);
        }
    }
}
