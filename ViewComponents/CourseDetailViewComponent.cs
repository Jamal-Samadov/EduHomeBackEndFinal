using EduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.ViewComponents
{
    public class CourseDetailViewComponent : ViewComponent
    {
        private readonly AppDbContext _dbContext;

        public CourseDetailViewComponent(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var courseDetail = await _dbContext.Courses.ToListAsync();
            return View(courseDetail);
        }
    }
}
