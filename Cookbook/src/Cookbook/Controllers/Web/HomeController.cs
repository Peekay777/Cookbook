using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Controllers.Web
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(RecipeController.Index), "Recipe");
            }

            return View();
        }
    }
}
