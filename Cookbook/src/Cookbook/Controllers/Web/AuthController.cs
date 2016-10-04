using Cookbook.Models;
using Cookbook.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Cookbook.Controllers.Web
{
    public class AuthController : Controller
    {
        private SignInManager<CookbookUser> _signInManager;
        private UserManager<CookbookUser> _userManager;
        private ILogger<AuthController> _log;

        public AuthController(
            SignInManager<CookbookUser> signInManager, 
            UserManager<CookbookUser> userManager, 
            ILogger<AuthController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _log = logger;
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Recipe");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new CookbookUser { UserName = model.UserName, Email = model.UserName };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _log.LogInformation(3, "User created a new account with password.");
                    return RedirectToAction(nameof(RecipeController.Index), "Home");
                }
                ModelState.AddModelError("", "Registration failed");
                return BadRequest(result.Errors);
            }

            return View(model);
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Recipe");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var signinResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);

                if (signinResult.Succeeded)
                {
                    return RedirectToAction(nameof(RecipeController.Index), "Recipe");
                }
            }

            ModelState.AddModelError("", "Login Failed");

            return View();
        }

        public async Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
