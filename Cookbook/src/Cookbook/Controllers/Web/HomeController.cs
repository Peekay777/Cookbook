using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Controllers.Web
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
