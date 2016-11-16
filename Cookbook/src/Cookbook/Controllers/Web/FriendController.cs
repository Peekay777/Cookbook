using Cookbook.Data;
using Cookbook.Models;
using Cookbook.Models.FriendViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cookbook.Controllers.Web
{
    [Authorize]
    public class FriendController : Controller
    {
        private ICookbookRepo _repo;
        private UserManager<CookbookUser> _userManager;

        public FriendController(ICookbookRepo repo, UserManager<CookbookUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public IActionResult MakeRequest()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeRequest(FindViewModel vm)
        {
            if (ModelState.IsValid)
            {
                CookbookUser friendUser = await _userManager.FindByEmailAsync(vm.Email);
                if (friendUser != null)
                {
                    CookbookUser currentUser = await _userManager.GetUserAsync(HttpContext.User);

                    if (currentUser.Id != friendUser.Id)
                    {
                        if (!await _repo.IsFriendAlready(currentUser.Id, friendUser.Id))
                        {
                            _repo.CreateFriendRequest(currentUser.Id, friendUser.Id);
                            if (await _repo.SaveChangesAsync())
                            {
                                return RedirectToAction(nameof(FriendController.Requests), "Friend");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "You are already friends");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Cannot friend request yourself");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email not found for user");
                }
            }
            return View(vm);
        }

        public async Task<IActionResult> Requests()
        {
            string userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;

            // Tuple<RequestId, UserId, UserStatus, FriendId, FriendStatus>
            var requests = await _repo.GetUserRequests(userId);
            var model = new RequestViewModel();

            foreach (var request in requests)
            {
                CookbookUser user = await _userManager.FindByIdAsync(request.Item4);
                if (request.Item5 == FriendStatus.Pending)
                {
                    model.PendingRequests.Add(new Request { RequestId = request.Item1, UserName = user.UserName });
                }
                else if (request.Item5 == FriendStatus.Requested)
                {
                    model.SentRequests.Add(new Request { RequestId = request.Item1, UserName = user.UserName });
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ChangeRequest(int id, int status)
        {
            if (await _repo.ChangeFriendRequestStatus(id, (FriendStatus)status))
            {
                await _repo.SaveChangesAsync();
            }

            return RedirectToAction(nameof(FriendController.Requests), "Friend");
        }
    }
}
