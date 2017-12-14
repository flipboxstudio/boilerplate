using System.Collections.Generic;
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

namespace App.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly TokenGenerator _tokenGenerator;

        private readonly UserManager<ApplicationUser> _userManager;

        private static readonly Dictionary<int, string> _messages = new Dictionary<int, string>
        {
            { 101, "Authentication success." },
            { 102, "Your account has been locked out." },
            { 103, "You are not allowed to log in to this service." },
            { 104, "Two Factor Authentication is required to log in to this service." },
            { 105, "Wrong email and / or password." },
            { 106, "Registration success." },
            { 107, "Registration failure." },
            { 108, "Authorized." },
        };

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
            var status = 101;
            var result = await _signInManager.PasswordSignInAsync(
                userName: request.Email,
                password: request.Password,
                isPersistent: false,
                lockoutOnFailure: true
            );

            if (result.Succeeded)
            {
                var user = _userManager.Users.SingleOrDefault(record => record.Email == request.Email);

                return new
                {
                    Message = _messages[status],
                    Status = status,
                    Data = new
                    {
                        Token = _tokenGenerator.GenerateToken(user)
                    }
                };
            }

            if (result.IsLockedOut)
            {
                status = 102;
            }
            else if (result.IsNotAllowed)
            {
                status = 103;
            }
            else if (result.RequiresTwoFactor)
            {
                status = 104;
            }
            else
            {
                status = 105;
            }

            throw new HttpException(HttpStatusCode.BadRequest)
            {
                Content = JsonConvert.SerializeObject(new
                {
                    Message = _messages[status],
                    Status = status
                })
            };
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
            var status = 106;
            var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
            var result = await _userManager.CreateAsync(user: user, password: request.Password);

            if (!result.Succeeded)
            {
                status = 107;

                throw new HttpException(HttpStatusCode.BadRequest)
                {
                    Content = JsonConvert.SerializeObject(new
                    {
                        Message = _messages[status],
                        Status = status,
                        Errors = result.Errors
                    }),
                };
            }

            await _signInManager.SignInAsync(user: user, isPersistent: false);

            return new {
                Message = _messages[status],
                Status = status,
                Data = new {
                    Token = _tokenGenerator.GenerateToken(user)
                }
            };
        }

        /// <summary>
        /// Get current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<object> Account() => new
        {
            Message = _messages[108],
            Status = 108,
            Data = new
            {
                Account = await _userManager.GetUserAsync(User)
            },
        };
    }
}