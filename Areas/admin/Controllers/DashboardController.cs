using Microsoft.AspNetCore.Mvc;

namespace EduHome.Areas.admin.Controllers
{
    public class DashboardController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
