#region using

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using App.Exceptions;
using App.Requests;
using App.Services.Auth;
using App.Services.Db.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApiContrib.Core.Filters;

#endregion

namespace App.Controllers.Api.v1
{
    [Route("api/v1/auth/[action]")]
    public class ApiV1AuthController : Controller
    {
        private readonly Dictionary<int, string> _authMessage = new Dictionary<int, string>
        {
            {801, "Authentication success."},
            {802, "Your account has been locked out."},
            {803, "You are not allowed to log in to this service."},
            {804, "Two Factor Authentication is required to log in to this service."},
            {805, "Wrong email and / or password."},
            {806, "Registration success."},
            {807, "Registration failure."},
            {808, "Authorized."}
        };

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly TokenGenerator _tokenGenerator;

        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        ///     Class constructor.
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="tokenGenerator"></param>
        /// <param name="userManager"></param>
        public ApiV1AuthController(
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
        ///     Authenticate a user.
        /// </summary>
        /// <param name="authenticationRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Validation]
        public async Task<object> Login([FromBody] AuthenticationRequest authenticationRequest)
        {
            var status = 801;
            var result = await _signInManager.PasswordSignInAsync(
                authenticationRequest.Email,
                authenticationRequest.Password,
                false,
                true
            );

            if (result.Succeeded)
            {
                var user = _userManager.Users.SingleOrDefault(record => record.Email == authenticationRequest.Email);

                return new
                {
                    Message = _authMessage[status],
                    Status = status,
                    Data = new
                    {
                        Token = _tokenGenerator.GenerateToken(user)
                    }
                };
            }

            if (result.IsLockedOut)
                status = 802;
            else if (result.IsNotAllowed)
                status = 803;
            else if (result.RequiresTwoFactor)
                status = 804;
            else
                status = 805;

            throw new HttpException(HttpStatusCode.BadRequest)
            {
                Content = JsonConvert.SerializeObject(new
                {
                    Message = _authMessage[status],
                    Status = status
                })
            };
        }

        /// <summary>
        ///     Register new user.
        /// </summary>
        /// <param name="registrationRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Validation]
        public async Task<object> Register([FromBody] RegistrationRequest registrationRequest)
        {
            var status = 806;
            var user = new ApplicationUser {UserName = registrationRequest.Email, Email = registrationRequest.Email};
            var result = await _userManager.CreateAsync(user, registrationRequest.Password);

            if (!result.Succeeded)
            {
                status = 807;

                throw new HttpException(HttpStatusCode.BadRequest)
                {
                    Content = JsonConvert.SerializeObject(new
                    {
                        Message = _authMessage[status],
                        Status = status,
                        result.Errors
                    })
                };
            }

            await _signInManager.SignInAsync(user, false);

            return new
            {
                Message = _authMessage[status],
                Status = status,
                Data = new
                {
                    Token = _tokenGenerator.GenerateToken(user),
                    User = user
                }
            };
        }

        /// <summary>
        ///     Get current user.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<object> Authorize()
        {
            return new
            {
                Message = _authMessage[808],
                Status = 808,
                Data = new
                {
                    User = await _userManager.GetUserAsync(User)
                }
            };
        }
    }
}