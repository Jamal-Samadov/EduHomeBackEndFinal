using EduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers
{
    public class TeachersController : Controller
    {
        private readonly AppDbContext _dbContext;
        public TeachersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int? id)
        {
            if (id is null) return NotFound();
            var teachers = _dbContext.Teachers.SingleOrDefault(x => x.Id == id);
            if (teachers.Id != id) return NotFound();
            return View(teachers);

        }
    }
}
