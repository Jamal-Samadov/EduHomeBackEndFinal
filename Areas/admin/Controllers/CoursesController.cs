using EduHome.Areas.admin.Data;
using EduHome.Areas.admin.Models;
using EduHome.DAL;
using EduHome.DAL.Entities;
using EduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.admin.Controllers
{
    public class CoursesController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public CoursesController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _dbContext.Courses.Where(x=>!x.IsDeleted).ToListAsync();
            return View(courses);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateModel model)
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

            var unicalFileName = await model.Image.GenerateFile(Constans.CoursePath);

            await _dbContext.Courses.AddAsync(new Course
            {
                Name = model.Name,
                About = model.About,
                SkillLevel = model.SkillLevel,
                Starts = model.Starts,
                Student = model.Student,
                Apply = model.Apply,
                Assesment = model.Assesment,
                Description = model.Description,
                CourseFee = model.CourseFee,
                Certification = model.Certification,
                ClassDuration = model.ClassDuration,
                Duration = model.Duration,
                Language = model.Language,
                Reply = model.Reply,
                ImageUrl = unicalFileName,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id == 0) return NotFound();
            var courses = await _dbContext.Courses.FindAsync(id);
            if (courses is null) return NotFound();
            return View(new CourseUpdateModel
            {
                ImageUrl = courses.ImageUrl,
                Name = courses.Name,
                SkillLevel = courses.SkillLevel,
                About = courses.About,
                Starts = courses.Starts,
                Student = courses.Student,
                Apply = courses.Apply,
                Certification = courses.Certification,
                ClassDuration = courses.ClassDuration,
                CourseFee = courses.CourseFee,
                Description = courses.Description,
                Duration = courses.Duration,
                Assesment = courses.Assesment,
                Language = courses.Language,
                Reply = courses.Reply,
            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, CourseUpdateModel model)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Courses.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(new CourseUpdateModel
                {
                    ImageUrl = existed.ImageUrl,
                    Assesment = existed.Assesment,
                    SkillLevel = existed.SkillLevel,
                    About = existed.About,
                    Apply = existed.Apply,
                    Certification = existed.Certification,
                    ClassDuration = existed.ClassDuration,
                    CourseFee = existed.CourseFee,
                    Description = existed.Description,
                    Duration = existed.Duration,
                    Language = existed.Language,
                    Name = existed.Name,
                    Reply = existed.Reply,
                    Starts = existed.Starts,
                    Student = existed.Student,
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

            var path = Path.Combine(Constans.RootPath, "assets", "img","course", existed.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            var unicalFileName = await model.Image.GenerateFile(Constans.CoursePath);

            existed.ImageUrl = unicalFileName;
            existed.About = model.About;
            existed.Student = model.Student;
            existed.SkillLevel = model.SkillLevel;
            existed.Starts = model.Starts;
            existed.Name = model.Name;
            existed.Apply = model.Apply;
            existed.Reply = model.Reply;
            existed.Language = model.Language;
            existed.Assesment = model.Assesment;
            existed.Duration = model.Duration;
            existed.ClassDuration = model.ClassDuration;
            existed.CourseFee = model.CourseFee;
            existed.Description = model.Description;
            existed.Certification = model.Certification;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var courses = await _dbContext.Courses.FindAsync(id);

            if (courses == null) return NotFound();
            return View(courses);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Courses.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();

            var path = Path.Combine(Constans.RootPath, "assets", "img","course", existed.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Courses.Remove(existed);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
