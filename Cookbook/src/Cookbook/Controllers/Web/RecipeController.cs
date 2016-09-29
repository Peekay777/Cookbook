using AutoMapper;
using Cookbook.Models;
using Cookbook.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Controllers.Web
{
    public class RecipeController : Controller
    {
        private ICookbookRepo _repo;

        public RecipeController(ICookbookRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var model = _repo.GetAll();

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var model = _repo.GetRecipe(id);
            var vm = Mapper.Map<RecipeViewModel>(model);

            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var model = _repo.GetRecipe(id);
            var vm = Mapper.Map<RecipeViewModel>(model);

            return View(vm);
        }
    }
}
