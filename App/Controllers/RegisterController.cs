#region using

using System.Threading.Tasks;
using App.Requests;
using App.Services.Db.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

#endregion

public class RegisterController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly SignInManager<ApplicationUser> _signInManager;

    public RegisterController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(RegistrationRequest registrationRequest)
    {
        if (!ModelState.IsValid)
        {
            return View(registrationRequest);
        }

        var user = new ApplicationUser { UserName = registrationRequest.Email, Email = registrationRequest.Email };
        var result = await _userManager.CreateAsync(user, registrationRequest.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(registrationRequest);
        }

        await _signInManager.SignInAsync(user, isPersistent: true);

        return Redirect("/");
    }
}
