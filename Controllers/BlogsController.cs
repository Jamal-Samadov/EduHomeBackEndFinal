using EduHome.DAL;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers
{
    public class BlogsController : Controller
    {
        private readonly AppDbContext _dbContext;


        public BlogsController(AppDbContext dbContext)
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
                Categories  = categories,
            };
            return View(blogViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null) return BadRequest();
            var blog = await _dbContext.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (blog is null) return NotFound();

            var tags = await _dbContext.Tags.Where(x => !x.IsDeleted).ToListAsync();
            var categories = await _dbContext.Categories.Where(x => !x.IsDeleted).ToListAsync();
            var blogViewModel = new BlogViewModel
            {
                Tags = tags,
                Blog = blog,
                Categories = categories,
            };
            return View(blogViewModel);
        }
    }
}
