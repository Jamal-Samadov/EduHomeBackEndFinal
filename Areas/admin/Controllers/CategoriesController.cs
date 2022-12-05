using EduHome.Areas.admin.Data;
using EduHome.Areas.admin.Models;
using EduHome.DAL;
using EduHome.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.admin.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public CategoriesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories.Where(x=>!x.IsDeleted).ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateModel model)
        {
            if (!ModelState.IsValid) return View();

            await _dbContext.Categories.AddAsync(new Category
            {
                CategoryName = model.CategoryName,
                Quantity = model.Quantity,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id == 0) return NotFound();
            var category = await _dbContext.Categories.FindAsync(id);
            if (category is null) return NotFound();
            return View(new CategoryUpdateModel
            {
                CategoryName = category.CategoryName,
                Quantity = category.Quantity,
            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, CategoryUpdateModel model)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Categories.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(new CategoryUpdateModel
                {
                    CategoryName = existed.CategoryName,
                    Quantity = existed.Quantity,
                });
            }

            existed.CategoryName = model.CategoryName;
            existed.Quantity = model.Quantity;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Categories.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();

            _dbContext.Categories.Remove(existed);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
