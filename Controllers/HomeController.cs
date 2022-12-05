using EduHome.DAL;
using EduHome.DAL.Entities;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EduHome.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;
        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _dbContext.Sliders.ToListAsync();
            var chooses = await _dbContext.Chooses.ToListAsync();
            var events = await _dbContext.Events.ToListAsync();
            var blogs = await _dbContext.Blogs.ToListAsync();
            var categories = await _dbContext.Categories.ToListAsync();
            var courses = await _dbContext.Courses.ToListAsync();
            var homeViewModel = new HomeViewModel
            {
                Sliders = sliders,
                Chooses = chooses,
                Events = events,
                Blogs = blogs,
                Categories = categories,
                Courses = courses,
            };
            return View(homeViewModel);
        }

        public IActionResult Search(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return NoContent();
            }
            var blogs = _dbContext.Blogs.Where(x => x.BlogName.ToLower().Contains(searchText.ToLower()))?.ToList();
            return PartialView("_SearchedProductPartial", blogs);
        }
    }
}