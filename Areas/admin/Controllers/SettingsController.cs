using EduHome.Areas.admin.Data;
using EduHome.Areas.admin.Models;
using EduHome.DAL;
using EduHome.DAL.Entities;
using EduHome.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.admin.Controllers
{
    public class SettingsController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public SettingsController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Setting> settings = await _dbContext.Settings.ToListAsync();
            return View(settings);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Setting model)
        {
            if (!ModelState.IsValid) return View();

            if (model.Photo != null)
            {
                if (model.Photo.ImageAllowed(2))
                {
                    model.Value = await model.Photo.GenerateFile(Constans.SettingPath);
                    await _dbContext.Settings.AddAsync(model);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Photo", "Zehmet olmasa 2 mb-n altinda sekil daxil edin");
                    return View();
                }
            }
            else
            {
                await _dbContext.Settings.AddAsync(model);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Update(int? id)
        {
            Setting existedSetting = await _dbContext.Settings.FirstOrDefaultAsync(s => s.Id == id);
            return View(existedSetting);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, Setting model)
        {
            if (!ModelState.IsValid) return View();
            Setting existedSetting = await _dbContext.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (existedSetting is null) return NotFound();
            if (existedSetting.Id != id) return BadRequest();

            if (model.Photo != null)
            {
                if (!model.Photo.ImageAllowed(2))
                {
                    string pathRoad = _env.WebRootPath + Constans.RootPath + existedSetting.Value;

                    if (System.IO.File.Exists(pathRoad))
                    {
                        System.IO.File.Delete(pathRoad);
                    }
                    existedSetting.Value = await model.Photo.GenerateFile(Constans.RootPath);

                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            existedSetting.Key = model.Key;
            existedSetting.Value = model.Value;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            Setting existedSetting = await _dbContext.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (id is null || id == 0) return NotFound();

            if (existedSetting is null) return NotFound();
            if (existedSetting.Id != id) return BadRequest();

            string path = Path.Combine(Constans.RootPath, "assets", "img", existedSetting.Value);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Settings.Remove(existedSetting);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {

            Setting existedSetting = await _dbContext.Settings.FirstOrDefaultAsync(s => s.Id == id);
            
            if (id is null || id == 0) return NotFound();


            if (existedSetting == null) return NotFound();
            return View(existedSetting);
        }
    }
}
