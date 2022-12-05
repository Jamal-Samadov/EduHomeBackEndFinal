using EduHome.Areas.admin.Data;
using EduHome.Areas.admin.Models;
using EduHome.DAL;
using EduHome.DAL.Entities;
using EduHome.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using NuGet.Protocol.Plugins;

namespace EduHome.Areas.admin.Controllers
{
    public class TeachersController : BaseController
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public TeachersController(AppDbContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _dbContext.Teachers.ToListAsync();
            return View(teachers);
        }
        

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeacherCreateModel model)
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

            var unicalFileName = await model.Image.GenerateFile(Constans.TeacherPath);

            await _dbContext.Teachers.AddAsync(new Teacher
            {
                TeacherFullName = model.TeacherFullName,
                Position = model.Position,
                About = model.About,
                Degree = model.Degree,
                Communication = model.Communication,
                Design = model.Design,
                Skype = model.Skype,
                Development = model.Development,
                Experience = model.Experience,
                Faculty = model.Faculty,
                Hobbies = model.Hobbies,
                Innovation = model.Innovation,
                Language = model.Language,
                Mail = model.Mail,
                Phone = model.Phone,
                TeamLeader = model.TeamLeader,
                ImageUrl = unicalFileName,
            });

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id is null || id == 0) return NotFound();
            var teachers = await _dbContext.Teachers.FindAsync(id);
            if (teachers is null) return BadRequest();
            return View(new TeacherUpdateModel
            {
                TeacherFullName = teachers.TeacherFullName,
                Position = teachers.Position,
                ImageUrl = teachers.ImageUrl,
                About = teachers.About,
                Communication = teachers.Communication,
                Skype = teachers.Skype,
                Degree = teachers.Degree,
                Design = teachers.Design,
                Development = teachers.Development,
                Faculty = teachers.Faculty,
                Experience = teachers.Experience,
                Hobbies = teachers.Hobbies,
                Innovation = teachers.Innovation,
                Language = teachers.Language,
                Mail = teachers.Mail,
                Phone = teachers.Phone,
                TeamLeader = teachers.TeamLeader,

            });
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, TeacherUpdateModel model)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Teachers.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(new TeacherUpdateModel
                {
                    ImageUrl = existed.ImageUrl,
                    TeacherFullName = existed.TeacherFullName,
                    Position = existed.Position,
                    About = existed.About,
                    Communication = existed.Communication,
                    Degree = existed.Degree,
                    Design = existed.Design,
                    Skype = existed.Skype,
                    Development = existed.Development,
                    Experience = existed.Experience,
                    Faculty = existed.Faculty,
                    Hobbies = existed.Hobbies,
                    Innovation = existed.Innovation,
                    Language = existed.Language,
                    Mail = existed.Mail,
                    Phone = existed.Phone,
                    TeamLeader = existed.TeamLeader,
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

            var path = Path.Combine(Constans.RootPath, "assets", "img", "teacher", existed.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            var unicalFileName = await model.Image.GenerateFile(Constans.TeacherPath);

            existed.ImageUrl = unicalFileName;
            existed.TeacherFullName = model.TeacherFullName;
            existed.Position = model.Position;
            existed.TeamLeader = model.TeamLeader;
            existed.About = model.About;
            existed.Degree = model.Degree;
            existed.Communication = model.Communication;
            existed.Design = model.Design;
            existed.Skype = model.Skype;
            existed.Development = model.Development;
            existed.Experience = model.Experience;
            existed.Faculty = model.Faculty;
            existed.Hobbies = model.Hobbies;
            existed.Innovation = model.Innovation;
            existed.Language = model.Language;
            existed.Mail = model.Mail;
            existed.Phone = model.Phone;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var teachers = await _dbContext.Teachers.FindAsync(id);

            if (teachers == null) return NotFound();
            return View(teachers);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.Teachers.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();

            var path = Path.Combine(Constans.RootPath, "assets", "img", "teacher", existed.ImageUrl);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

            _dbContext.Teachers.Remove(existed);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
