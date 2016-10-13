using AutoMapper;
using Cookbook.Data;
using Cookbook.Models.RecipeViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cookbook.Controllers.Web
{
    [Authorize]
    public class RecipeController : Controller
    {
        private ICookbookRepo _repo;

        public RecipeController(ICookbookRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var model = _repo.GetAllByUser(User.Identity.Name);

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
