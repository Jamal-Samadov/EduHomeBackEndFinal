using EduHome.Areas.admin.Data;
using EduHome.Areas.admin.Models;
using EduHome.DAL;
using EduHome.DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.admin.Controllers
{
    public class ChoosesController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public ChoosesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var chooses = await _dbContext.Chooses.ToListAsync();
            return View(chooses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ChooseCreateModel model)
        {
            if (!ModelState.IsValid) return View();

            await _dbContext.Chooses.AddAsync(new Choose
            {
                MainTitle = model.MainTitle,
                Subtitle = model.Subtitle,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id == 0) return NotFound();
            var chooses = await _dbContext.Chooses.FindAsync(id);
            if (chooses is null) return NotFound();
            return View(new ChooseUpdateModel
            {
                MainTitle = chooses.MainTitle,
                Subtitle = chooses.Subtitle,
            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, ChooseUpdateModel model)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Chooses.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(new ChooseUpdateModel
                {
                    MainTitle = existed.MainTitle,
                    Subtitle = existed.Subtitle,
                });
            }
            existed.MainTitle = model.MainTitle;
            existed.Subtitle = model.Subtitle;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var chooses = await _dbContext.Chooses.FindAsync(id);

            if (chooses == null) return NotFound();
            return View(chooses);
        }


        [HttpPost]
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Chooses.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();



            _dbContext.Chooses.Remove(existed);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
