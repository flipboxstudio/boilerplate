#region using

using System.Linq;
using System.Threading.Tasks;
using App.Requests;
using App.Services.Db.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace App.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
        )
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AuthenticationRequest authenticationRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(authenticationRequest);
            }

            var result = await _signInManager.PasswordSignInAsync(
                authenticationRequest.Email,
                authenticationRequest.Password,
                false,
                true
            );

            if (result.Succeeded)
            {
                // Just redirect to our index after logging in.
                return Redirect("/");
            }

            ModelState.AddModelError(string.Empty, "Invalid email and / or password.");

            return View(authenticationRequest);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Auth");
        }
    }
}