using Cookbook.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cookbook.Data
{
    public interface ICookbookRepo
    {
        IEnumerable<Recipe> GetAll();
        IEnumerable<Recipe> GetAllByUser(string name);
        Recipe GetRecipe(int id);

        void AddRecipe(Recipe recipe);

        bool EditRecipe(int id, Recipe recipe, string userName);

        bool DeleteRecipe(int id);

        Task<bool> SaveChangesAsync();
        
    }
}
