using Cookbook.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Controllers
{
    public class RecipeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RecipeViewModel model)
        {
            return View();
        }
    }
}
