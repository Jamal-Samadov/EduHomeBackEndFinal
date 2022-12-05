using EduHome.Areas.admin.Data;
using EduHome.Areas.admin.Models;
using EduHome.DAL;
using EduHome.DAL.Entities;
using EduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGeneration;

namespace EduHome.Areas.admin.Controllers
{
    public class SlidersController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public SlidersController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _dbContext.Sliders.ToListAsync();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateModel model)
        {
                if (!ModelState.IsValid) return View();

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

            var unicalFileName = await model.Image.GenerateFile(Constans.SliderPath);

            await _dbContext.Sliders.AddAsync(new Slider
            {
                Headtitle = model.Headtitle,
                Subtitle = model.Subtitle,
                ImageUrl = unicalFileName,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id == 0) return NotFound();
            var slider = await _dbContext.Sliders.FindAsync(id);
            if (slider is null) return NotFound();
            return View(new SliderUpdateModel
            {
                ImageUrl =slider.ImageUrl,
                Subtitle = slider.Subtitle,
                Headtitle = slider.Headtitle,
            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, SliderUpdateModel model)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Sliders.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(new SliderUpdateModel
                {
                    ImageUrl = existed.ImageUrl,
                    Subtitle = existed.Subtitle,
                    Headtitle = existed.Headtitle,
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

            var path = Path.Combine(Constans.RootPath, "assets", "img","slider", existed.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            var unicalFileName = await model.Image.GenerateFile(Constans.SliderPath);

            existed.ImageUrl = unicalFileName;
            existed.Subtitle = model.Subtitle;
            existed.Headtitle = model.Headtitle;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Sliders.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();

            var path = Path.Combine(Constans.RootPath, "assets", "img","slider", existed.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Sliders.Remove(existed);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var sliders = await _dbContext.Sliders.FindAsync(id);

            if (sliders == null) return NotFound();
            return View(sliders);
        }
    }
}
