using Cookbook.Models;
using Cookbook.Models.AuthViewModels;
using Cookbook.Services;
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
        private IEmailSender _emailSender;

        public AuthController(
            SignInManager<CookbookUser> signInManager, 
            UserManager<CookbookUser> userManager, 
            ILogger<AuthController> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _log = logger;
            _emailSender = emailSender;
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
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    await _emailSender.SendEmailAsync(
                        user.Email,
                        "Confirm your email address for Cookbook", 
                        $"Hi,\nThank you for registering for a Cookbook account\nPlease confirm your email\n<a href='{callbackUrl}'>Confirm Email</a>");
                    //await _signInManager.SignInAsync(user, isPersistent: true);

                    _log.LogInformation(3, "User created a new account with password.");
                    return RedirectToAction(nameof(AuthController.RegisterConfirmEmail), "Auth", user.Email);
                }

                AddErrors(result);
            }

            return View(model);
        }

        public IActionResult RegisterConfirmEmail()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Recipe");
            }
            return View();
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(RecipeController.Index), "Recipe");
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
                var user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "You must have confirmed your email address to log in.");
                        return View(model);
                    }
                }

                var signinResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
                if (signinResult.Succeeded)
                {
                    return RedirectToAction(nameof(RecipeController.Index), "Recipe");
                }
            }

            ModelState.AddModelError(string.Empty, "Login Failed");

            return View();
        }

        public async Task<ActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction(nameof(RecipeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");   // TODO redirect
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");   // TODO redirect
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }


        #region Helpers

        private void AddErrors (IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}
