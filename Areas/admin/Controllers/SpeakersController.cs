using EduHome.Areas.admin.Data;
using EduHome.Areas.admin.Models;
using EduHome.DAL;
using EduHome.DAL.Entities;
using EduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.admin.Controllers
{
    public class SpeakersController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public SpeakersController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var speakers = await _dbContext.Speakers.ToListAsync();
            return View(speakers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpeakerCreateModel model)
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
                return View();
            }

            if (!model.Image.ImageAllowed(2))
            {
                ModelState.AddModelError("Image", "Şəkil həcmi max 2 mb ola bilər");
                return View();
            }

            var unicalFileName = await model.Image.GenerateFile(Constans.SpeakerPath);

            await _dbContext.Speakers.AddAsync(new Speaker
            {
                Name = model.Name,
                Surname = model.Surname,
                Job = model.Job,
                Position = model.Position,
                ImageUrl = unicalFileName,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id == 0) return NotFound();
            var speakers = await _dbContext.Speakers.FindAsync(id);
            if (speakers is null) return NotFound();
            return View(new SpeakerUpdateModel
            {
                ImageUrl = speakers.ImageUrl,
                Name = speakers.Name,
                Surname = speakers.Surname,
                Job = speakers.Job,
                Position = speakers.Position,
            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, SpeakerUpdateModel model)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Speakers.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(new SpeakerUpdateModel
                {
                    ImageUrl = existed.ImageUrl,
                    Name = existed.Name,
                    Surname = existed.Surname,
                    Job = existed.Job,
                    Position = existed.Position,
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

            var path = Path.Combine(Constans.RootPath, "assets", "img","speaker", existed.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            var unicalFileName = await model.Image.GenerateFile(Constans.SpeakerPath);

            existed.ImageUrl = unicalFileName;
            existed.Name = model.Name;
            existed.Surname = model.Surname;
            existed.Job = model.Job;
            existed.Position = model.Position;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Speakers.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();

            var path = Path.Combine(Constans.RootPath, "assets", "img","speaker", existed.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Speakers.Remove(existed);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var speakers = await _dbContext.Speakers.FindAsync(id);

            if (speakers == null) return NotFound();
            return View(speakers);
        }
    }
}
