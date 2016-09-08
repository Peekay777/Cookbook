using AutoMapper;
using Cookbook.Models;
using Cookbook.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cookbook.Controllers.Api
{
    [Route("api/[controller]")]
    public class RecipeController : Controller
    {
        private ICookbookRepo _repo;

        public RecipeController(ICookbookRepo repo)
        {
            _repo = repo;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            var recipes = _repo.GetAll();

            return Ok(recipes);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var recipe = _repo.GetRecipe(id);
                return Ok(Mapper.Map<RecipeViewModel>(recipe));
            }
            catch (Exception ex)
            {
                // TODO add logger
                return BadRequest("Error occurred");
            }
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RecipeViewModel theRecipe)
        {
            if (ModelState.IsValid)
            {
                var newRecipe = Mapper.Map<Recipe>(theRecipe);
                _repo.AddRecipe(newRecipe);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"api/recipe/{newRecipe.Id}", Mapper.Map<RecipeViewModel>(newRecipe));
                }
            }

            return BadRequest("Failed to save the recipe");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]RecipeViewModel theRecipe)
        {

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
               // TODO add logger
            }

            return BadRequest("Failed to delete the recipe");
        }
    }
}
