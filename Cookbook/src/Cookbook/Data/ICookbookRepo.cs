using Cookbook.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Cookbook.Data
{
    public interface ICookbookRepo
    {
        Task<bool> SaveChangesAsync();

        #region Recipes DAO

        IEnumerable<Recipe> GetAll();
        IEnumerable<Recipe> GetAllByUser(string name);
        Recipe GetRecipe(int id);

        void AddRecipe(Recipe recipe);
        bool EditRecipe(int id, Recipe recipe, string userName);
        bool DeleteRecipe(int id);

        #endregion

        #region Friends DAO

        Task<bool> IsFriendAlready(string id1, string id2);
        void CreateFriendRequest(string id1, string id2);
        Task<List<Tuple<int, string, FriendStatus, string, FriendStatus>>> GetUserRequests(string userId);
        Task<bool> ChangeFriendRequestStatus(int id, FriendStatus status);

        #endregion
    }
}
