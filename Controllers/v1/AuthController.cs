using App.Response.v1;
using App.Request;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using MimeKit;
using UserModel = App.Model.User;

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
            Database database,
            Mailer mailer,
            Authenticator authenticator)
        {
            _database = database;
            _mailer = mailer;
            _authenticator = authenticator;
        }

        /// <summary>
        /// Register new User.
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
                "Welcome to App",
                "hello.html",
                new {
                    baseUrl = Url.AbsoluteContent(""),
                    subject = "Welcome to App",
                    nickName = user.NickName
                }
            );

            return new Registered
            {
                Message = "OK",
                Data = user
            };
        }

        /// <summary>
        /// Authenticate a User.
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
                Data = _authenticator.Authenticate(authRequest)
            };
        }

        /// <summary>
        /// Send email to user about forgot password.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("forgot", Name = "v1.auth.forgot")]
        public Forgot Forgot([FromBody] AuthForgot request)
        {
            this.ValidateRequest();

            var newPassword = "".Random(8);
            var user = _database.FindUserByEmail(request.Email)
                                .Forgot(newPassword);

            _mailer.SendEmail(
                new MailboxAddress(user.FullName, user.Email),
                "Password Reset",
                "password.html",
                new {
                    baseUrl = Url.AbsoluteContent(""),
                    subject = "Password Reset",
                    newPassword
                }
            );

            user = _database.UpdateUser(user);

            return new Forgot
            {
                Message = "OK",
                Data = user
            };
        }

        /// <summary>
        /// Get current authenticated user.
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
