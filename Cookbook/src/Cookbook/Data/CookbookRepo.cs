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
            Recipe recipe = GetRecipe(id);

            if (recipe == null)
            {
                return false;
            }
            else
            {
                recipe.Name = newRecipe.Name;
                recipe.Serves = newRecipe.Serves;
                recipe.UserName = userName;
                recipe.Ingredients = newRecipe.Ingredients;
                recipe.Method = newRecipe.Method;

                _context.Entry(recipe).State = EntityState.Modified;
                return true;
            }
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
              .Include(r => r.Ingredients)
              .Include(r => r.Method)
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
