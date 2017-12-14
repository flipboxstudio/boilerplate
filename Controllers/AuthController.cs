using System.Linq;
using System.Net;
using System.Threading.Tasks;
using App.Attributes;
using App.Exceptions;
using App.Requests;
using App.Services.Auth;
using App.Services.Db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace JWT.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly TokenGenerator _tokenGenerator;

        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="tokenGenerator"></param>
        /// <param name="userManager"></param>
        public AuthController(
            SignInManager<ApplicationUser> signInManager,
            TokenGenerator tokenGenerator,
            UserManager<ApplicationUser> userManager
        )
        {
            _signInManager = signInManager;
            _tokenGenerator = tokenGenerator;
            _userManager = userManager;
        }

        /// <summary>
        /// Authenticate a user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateRequest]
        public async Task<object> Login([FromBody] AuthenticationRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(
                userName: request.Email,
                password: request.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
                throw new HttpException(HttpStatusCode.BadRequest)
                {
                    Content = JsonConvert.SerializeObject(new { Message = "Wrong email and/or password." })
                };

            var user = _userManager.Users.SingleOrDefault(record => record.Email == request.Email);

            return new { Token = _tokenGenerator.GenerateToken(user) };
        }

        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateRequest]
        public async Task<object> Register([FromBody] RegistrationRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user: user, password: request.Password);

            if (!result.Succeeded)
                throw new HttpException(HttpStatusCode.InternalServerError)
                {
                    Content = JsonConvert.SerializeObject(new { Errors = result.Errors }),
                };

            await _signInManager.SignInAsync(user: user, isPersistent: false);

            return new { Token = _tokenGenerator.GenerateToken(user) };
        }

        /// <summary>
        /// Get current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ApplicationUser> Account()
        {
            return await _userManager.GetUserAsync(User);
        }
    }
}