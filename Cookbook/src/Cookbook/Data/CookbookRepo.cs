using Cookbook.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookbook.Data
{
    public class CookbookRepo : ICookbookRepo
    {
        private CookbookContext _context;

        public CookbookRepo(CookbookContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Add a recipe
        /// </summary>
        /// <param name="recipe"></param>
        public void AddRecipe(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
        }
        /// <summary>
        /// Edit recipe
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recipe"></param>
        public bool EditRecipe(int id, Recipe newRecipe, string userName)
        {
            Recipe oldRecipe = GetRecipe(id);

            if (oldRecipe != null)
            {
                oldRecipe.Name = newRecipe.Name;
                oldRecipe.Serves = newRecipe.Serves;
                oldRecipe.UserName = userName;

                // Delete child collections
                foreach (var existingChild in oldRecipe.Ingredients.ToList())
                {
                    if (!newRecipe.Ingredients.Any(c => c.Id == existingChild.Id))
                        _context.Ingredients.Remove(existingChild);
                }

                foreach (var existingChild in oldRecipe.Method.ToList())
                {
                    if (!newRecipe.Method.Any(c => c.Id == existingChild.Id))
                        _context.Method.Remove(existingChild);
                }


                // Update and Insert Children
                List<Ingredient> ingredients = new List<Ingredient>();
                foreach (var childModel in newRecipe.Ingredients)
                {
                    var existingChild = oldRecipe.Ingredients
                        .Where(c => c.Id == childModel.Id)
                        .SingleOrDefault();

                    if (existingChild != null)
                        // Update child
                        ingredients.Add(childModel);
                    else
                    {
                        // Insert child
                        var newChild = new Ingredient
                        {
                            Order = childModel.Order,
                            Description = childModel.Description,
                            RecipeId = childModel.RecipeId
                        };
                        ingredients.Add(newChild);
                    }
                }
                oldRecipe.Ingredients = ingredients;

                List<Instruction> instructions = new List<Instruction>();
                foreach (var childModel in newRecipe.Method)
                {
                    var existingChild = oldRecipe.Method
                        .Where(c => c.Id == childModel.Id)
                        .SingleOrDefault();

                    if (existingChild != null)
                        // Update child
                        instructions.Add(childModel);
                    else
                    {
                        // Insert child
                        var newChild = new Instruction
                        {
                            Order = childModel.Order,
                            Task = childModel.Task,
                            RecipeId = childModel.RecipeId
                        };
                        instructions.Add(newChild);
                    }
                }
                oldRecipe.Method = instructions;


                _context.Entry(oldRecipe).State = EntityState.Modified;
                return true;
            }

            return false;
        }
        /// <summary>
        /// Delete Recipe
        /// </summary>
        /// <param name="id"></param>
        public bool DeleteRecipe(int id)
        {
            var recipe = _context.Recipes
              .Include(r => r.Ingredients)
              .Include(r => r.Method)
              .Where(r => r.Id == id)
              .FirstOrDefault();

            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Get a recipe with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Recipe GetRecipe(int id)
        {
            return _context.Recipes
              .Include(i => i.Ingredients)
              .Include(m => m.Method)
              .Where(r => r.Id == id)
              .DefaultIfEmpty(null)
              .FirstOrDefault();
        }
        /// <summary>
        /// Get all recipes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Recipe> GetAll()
        {
            return _context.Recipes.ToList();
        }
        /// <summary>
        /// Get all recipes for a user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public IEnumerable<Recipe> GetAllByUser(string userName)
        {
            return _context.Recipes
                .Where(r => r.UserName == userName)
                .ToList();
        }
        /// <summary>
        /// Save database changes
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

    }
}
