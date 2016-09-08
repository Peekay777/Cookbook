using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookbook.Models
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
            _context.Add(recipe);
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
        /// Get all recipes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Recipe> GetAll()
        {
            return _context.Recipes.ToList();
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
              .FirstOrDefault();
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
