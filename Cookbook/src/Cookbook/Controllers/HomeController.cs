using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
