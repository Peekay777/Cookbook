using Cookbook.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cookbook.Data
{
    public interface ICookbookRepo
    {
        Task<bool> SaveChangesAsync();

        IEnumerable<Recipe> GetAll();
        IEnumerable<Recipe> GetAllByUser(string name);
        Recipe GetRecipe(int id);

        void AddRecipe(Recipe recipe);

        bool EditRecipe(int id, Recipe recipe, string userName);

        bool DeleteRecipe(int id);
    }
}
