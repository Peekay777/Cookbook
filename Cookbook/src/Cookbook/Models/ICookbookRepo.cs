using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cookbook.Models
{
    public interface ICookbookRepo
    {
        IEnumerable<Recipe> GetAll();
        Recipe GetRecipe(int id);

        void AddRecipe(Recipe recipe);

        bool EditRecipe(int id, Recipe recipe);

        bool DeleteRecipe(int id);

        Task<bool> SaveChangesAsync();
    }
}
