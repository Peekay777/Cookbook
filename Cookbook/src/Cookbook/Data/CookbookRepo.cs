using Cookbook.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Cookbook.Models.FriendViewModels;

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
        /// Save database changes
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
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
                oldRecipe.IsPrivate = newRecipe.IsPrivate;
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
        /// Check to see if user is already friends
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendUserId"></param>
        /// <returns></returns>
        public async Task<bool> IsFriendAlready(string userId, string friendUserId)
        {
            // select u.Id
            // from AspNetUsers u
            // where not u.Id in (
            //    select f2.UserId
            //    from Friends f
            //    join Friends f2 on f.RequestId = f2.RequestId and f.UserId = '67aae632-f7e4-43c8-a662-1c3d3045fed8')
            return await _context.Friends
                .Where(f => f.UserId == userId)
                .Join(_context.Friends,
                    f => f.RequestId,
                    f2 => f2.RequestId,
                    (f, f2) => new { UserId = f2.UserId })
                .Distinct()
                .ContainsAsync(new { UserId = friendUserId });
        }
        /// <summary>
        /// Create a friend request
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="friendUserId"></param>
        public void CreateFriendRequest(string userId, string friendUserId)
        {
            _context.FriendRequests.Add(new FriendRequest
            {
                Friends = new List<Friend> {
                    new Friend { UserId = userId, Status = FriendStatus.Pending },
                    new Friend { UserId = friendUserId, Status = FriendStatus.Requested }
                }
            });
        }
        /// <summary>
        /// Gets the friend requests sent or pending for a user
        /// Returns a Tuple with RequestId, UserId, UserStatus, FriendId, FriendStatus
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Tuple<int, string, FriendStatus, string, FriendStatus>>> GetUserRequests(string userId)
        {
            //select f.RequestId, f.UserId as UserId, f.Status as UserStatus, f2.UserId as FriendId, f2.Status as FriendStatus
            //from Friends f
            //join Friends f2 on f.RequestId = f2.RequestId
            //and f.UserId = userId
            //and f.Status != f2.Status
            return await _context.Friends
                .Join(_context.Friends,
                    f => f.RequestId,
                    f2 => f2.RequestId,
                    (f, f2) => new { f, f2 })
                .Where(fs => fs.f.UserId == userId && fs.f.Status != fs.f2.Status)
                .Select(s => Tuple.Create(s.f.RequestId, s.f.UserId, s.f.Status, s.f2.UserId, s.f2.Status))
                .ToListAsync();
        }

        public async Task<bool> ChangeFriendRequestStatus(int id, FriendStatus status)
        {
            FriendRequest friendRequest = await _context.FriendRequests
                .Include(fr => fr.Friends)
                .Where(fr => fr.RequestId == id)
                .SingleOrDefaultAsync();

            if (friendRequest != null)
            {
                foreach (var friend in friendRequest.Friends)
                {
                    friend.Status = status;
                }
                _context.Entry(friendRequest).State = EntityState.Modified;
                return true;
            }

            return false;
        }
    }
}
