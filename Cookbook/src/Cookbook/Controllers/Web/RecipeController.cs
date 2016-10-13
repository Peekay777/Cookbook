using System;
using AutoMapper;
using Cookbook.Models;
using Cookbook.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cookbook.Data;

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
