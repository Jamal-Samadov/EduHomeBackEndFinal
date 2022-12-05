using EduHome.Areas.admin.Data;
using EduHome.Areas.admin.Models;
using EduHome.DAL;
using EduHome.DAL.Entities;
using EduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.admin.Controllers
{
    public class BlogsController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public BlogsController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _dbContext.Blogs.ToListAsync();
            return View(blogs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                return Ok(errorList);
            }

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Şəkil seçilməlidir");
                return View(model);
            }

            if (!model.Image.ImageAllowed(2))
            {
                ModelState.AddModelError("Image", "Şəkil həcmi max 2 mb ola bilər");
                return View(model);
            }

            var unicalFileName = await model.Image.GenerateFile(Constans.BlogPath);

            await _dbContext.Blogs.AddAsync(new Blog
            {
                BlogName = model.BlogName,
                BlogDescription = model.BlogDescription,
                Reply = model.Reply,
                ImageUrl = unicalFileName,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id == 0) return NotFound();
            var blogs = await _dbContext.Blogs.FindAsync(id);
            if (blogs is null) return NotFound();
            return View(new BlogUpdateModel
            {
                ImageUrl = blogs.ImageUrl,
                BlogName = blogs.BlogName,
                BlogDescription = blogs.BlogDescription,
                Reply = blogs.Reply,
            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, BlogUpdateModel model)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Blogs.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(new BlogUpdateModel
                {
                    ImageUrl = existed.ImageUrl,
                    BlogName = existed.BlogName,
                    BlogDescription = existed.BlogDescription,
                    Reply = existed.Reply,
                });
            }

            if (!model.Image.IsImage())
            {
                ModelState.AddModelError("Image", "Şəkil seçilməlidir");
                return View();
            }

            if (!model.Image.ImageAllowed(2))
            {
                ModelState.AddModelError("Image", "Şəkil həcmi max 2 mb ola bilər");
                return View();
            }

            var path = Path.Combine(Constans.RootPath, "assets", "img", "blog", existed.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            var unicalFileName = await model.Image.GenerateFile(Constans.RootPath);

            existed.ImageUrl = unicalFileName;
            existed.BlogName = model.BlogName;
            existed.BlogDescription = model.BlogDescription;
            existed.Reply = model.Reply;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var blogs = await _dbContext.Blogs.FindAsync(id);

            if (blogs == null) return NotFound();
            return View(blogs);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Blogs.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();

            var path = Path.Combine(Constans.RootPath, "assets", "img", "blog", existed.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Blogs.Remove(existed);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
