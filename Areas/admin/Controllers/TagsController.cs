using EduHome.Areas.admin.Models;
using EduHome.DAL;
using EduHome.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.admin.Controllers
{
    public class TagsController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public TagsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _dbContext.Tags.Where(x => !x.IsDeleted).ToListAsync();
            return View(tags);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TagsCreateModel model)
        {
            if (!ModelState.IsValid) return View();

            await _dbContext.Tags.AddAsync(new Tags
            {
                TagsName = model.TagsName,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id == 0) return NotFound();
            var tags = await _dbContext.Tags.FindAsync(id);
            if (tags is null) return NotFound();
            return View(new TagsUpdateModel
            {
                TagsName = tags.TagsName,
            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, TagsUpdateModel model)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Tags.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(new TagsUpdateModel
                {
                    TagsName = existed.TagsName,
                });
            }

            existed.TagsName = model.TagsName;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var tags = await _dbContext.Tags.FindAsync(id);

            if (tags == null) return NotFound();
            return View(tags);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Tags.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();

            _dbContext.Tags.Remove(existed);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
