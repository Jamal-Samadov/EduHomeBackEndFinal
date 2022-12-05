using EduHome.DAL;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _dbContext;
        public CoursesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var courses = await _dbContext.Courses.Where(x => !x.IsDeleted).ToListAsync();
            return View(courses);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return BadRequest();
            var course = await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course is null) return NotFound();

            var tags = await _dbContext.Tags.Where(x => !x.IsDeleted).ToListAsync();
            var categories = await _dbContext.Categories.Where(x => !x.IsDeleted).ToListAsync();
            var courseViewModel = new CourseViewModel
            {
                Tags = tags,
                Course = course,
                Categories = categories,
            };
            return View(courseViewModel);


        }
    }
}
