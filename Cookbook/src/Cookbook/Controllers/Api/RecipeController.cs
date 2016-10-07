using AutoMapper;
using Cookbook.Models;
using Cookbook.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Cookbook.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize]
    public class RecipeController : Controller
    {
        private ICookbookRepo _repo;
        private ILogger<RecipeController> _log;

        public RecipeController(ICookbookRepo repo, ILogger<RecipeController> logger)
        {
            _repo = repo;
            _log = logger;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            var recipes = _repo.GetAllByUser(GetUserIdentityName());

            return Ok(recipes);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var recipe = _repo.GetRecipe(id);

                if (recipe != null)
                {
                    return Ok(Mapper.Map<RecipeViewModel>(recipe));
                }
            }
            catch (Exception ex)
            {
                _log.LogError("Error finding recipe {0}:\n {1}", id, ex);
            }

            return BadRequest("Recipe not found");
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RecipeViewModel theRecipe)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newRecipe = Mapper.Map<Recipe>(theRecipe);
                    newRecipe.UserName = GetUserIdentityName();
                    _repo.AddRecipe(newRecipe);

                    if (await _repo.SaveChangesAsync())
                    {
                        return Created($"api/recipe/{newRecipe.Id}", Mapper.Map<RecipeViewModel>(newRecipe));
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError("Error saving a recipe {0}:\n {1}", theRecipe.Name, ex);
            }

            return BadRequest("Failed to save the recipe");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]RecipeViewModel theRecipe)
        {
            try
            {
                if (ModelState.IsValid && id == theRecipe.Id)
                {
                    var newRecipe = Mapper.Map<Recipe>(theRecipe);
                    if (_repo.EditRecipe(id, newRecipe, GetUserIdentityName()))
                    {
                        if (await _repo.SaveChangesAsync())
                        {
                            return NoContent();
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError("Error saving a recipe {0}:\n {1}", theRecipe.Name, ex);
            }

            return BadRequest("Failed to save the recipe");
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (_repo.DeleteRecipe(id))
                {
                    if (await _repo.SaveChangesAsync())
                    {
                        return NoContent();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _log.LogError("Error deleting the recipe {0}:\n {1}", id, ex);
            }

            return BadRequest("Failed to delete the recipe");
        }

        public virtual string GetUserIdentityName()
        {
            return User.Identity.Name;
        }
    }
}