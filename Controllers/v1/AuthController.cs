using App.Exceptions;
using App.Options;
using App.Response.v1;
using App.Request;
using App.Services;
using static BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using MimeKit;

namespace Boilerplate.Controllers.v1
{
    [Route("v1/[controller]")]
    public class AuthController : Controller
    {
        /// <summary>
        /// Database instance, used to save newly registered user.
        /// </summary>
        private readonly Database _database;

        /// <summary>
        /// Mailer instance, used to sending email when user want to reset their password.
        /// </summary>
        private readonly Mailer _mailer;

        /// <summary>
        /// JWT Config, we need to tell client when the token valid until.
        /// </summary>
        private readonly JwtConfig _jwtConfig;

        /// <summary>
        /// Authenticator instance, responsible to authenticate user and generate token.
        /// </summary>
        private readonly Authenticator _authenticator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="jwtConfig"></param>
        /// <param name="database"></param>
        /// <param name="mailer"></param>
        /// <param name="authenticator"></param>
        public AuthController(
            IOptions<JwtConfig> jwtConfig,
            Database database,
            Mailer mailer,
            Authenticator authenticator)
        {
            _jwtConfig = jwtConfig.Value;
            _database = database;
            _mailer = mailer;
            _authenticator = authenticator;
        }

        /// <summary>
        ///     Register new User.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register", Name = "v1.auth.register")]
        public Registered Register([FromBody] AuthRegister request)
        {
            this.ValidateRequest();

            var user = _database.AddUser(
                App.Model.User.CreateFromRequest(request)
            );

            _mailer.SendEmail(
                new MailboxAddress(user.FullName, user.Email),
                "Welcome",
                $@"Dear {user.NickName},

Welcome to our App!

Regards,
Robot"
            );

            return new Registered {
                Message = "OK",
                Data = user
            };
        }

        /// <summary>
        ///     Authenticate a User.
        /// </summary>
        /// <param name="authRequest"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login", Name = "v1.auth.login")]
        public Authenticated Login([FromBody] AuthRequest authRequest)
        {
            this.ValidateRequest();

            return new Authenticated
            {
                Message = "OK",
                Data = new AuthData
                {
                    AccessToken = _authenticator.Authenticate(authRequest),
                    ExpiresIn = (int) _jwtConfig.ValidFor.TotalSeconds,
                    User = _authenticator.User
                }
            };
        }

        /// <summary>
        ///     Send email to user about forgot password.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("forgot", Name = "v1.auth.forgot")]
        public Forgot Forgot([FromBody] AuthForgot request)
        {
            var user = _database.FindUserByEmail(request.Email);
            var newPassword = "".Random(8);

            _mailer.SendEmail(
                new MailboxAddress(user.FullName, user.Email),
                "Password Reset",
                $@"Dear {user.NickName},

Here's your new password: {newPassword}. You need to change this password once you signed in to application.

Regards,
Robot"
            );

            user.Password = HashPassword(newPassword);
            user.NeedToChangePassword = true;

            _database.UpdateUser(user);

            return new Forgot {
                Message = "OK",
                Data = user
            };
        }

        /// <summary>
        ///     Protected route for authenticated authRequest (any Role is OK).
        /// </summary>
        /// <returns></returns>
        [HttpGet("user", Name = "v1.auth.user")]
        public Profile GetUser()
        {
            return new Profile
            {
                Message = "OK",
                Data = this.GetCurrentUser()
            };
        }
    }
}