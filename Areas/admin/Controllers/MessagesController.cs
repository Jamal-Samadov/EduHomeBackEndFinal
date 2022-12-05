using EduHome.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.admin.Controllers
{
    public class MessagesController : BaseController
    {
        private readonly AppDbContext _dbContext;

        public MessagesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var messages = await _dbContext.ContactMessages.Where(x => !x.IsDeleted).ToListAsync();
            return View(messages);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var messages = await _dbContext.ContactMessages.FindAsync(id);

            if (messages == null) return NotFound();
            return View(messages);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();

            var existed = await _dbContext.ContactMessages.FindAsync(id);
            if (existed is null) return NotFound();
            if (existed.Id != id) return BadRequest();

            _dbContext.ContactMessages.Remove(existed);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
