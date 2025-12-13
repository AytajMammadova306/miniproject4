using Microsoft.AspNetCore.Mvc;

namespace MiniProject.Area.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
