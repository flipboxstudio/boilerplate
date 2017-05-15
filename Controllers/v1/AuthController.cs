using App.Exceptions;
using App.Model;
using App.Options;
using App.Response;
using App.Response.v1;
using App.Request;
using App.Services;
using static BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Boilerplate.Controllers.v1
{
    [Route("v1/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMemoryCache _cache;

        private readonly Database _database;

        private readonly JwtConfig _jwtConfig;

        /// <summary>
        ///     Just a nice constructor.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="jwtConfig"></param>
        /// <param name="database"></param>
        public AuthController(IMemoryCache cache, IOptions<JwtConfig> jwtConfig, Database database)
        {
            _cache = cache;
            _jwtConfig = jwtConfig.Value;
            _database = database;
        }

        /// <summary>
        ///     Authenticate a User.
        /// </summary>
        /// <param name="authRequest"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login", Name = "auth.login")]
        public IActionResult Login([FromBody] AuthRequest authRequest)
        {
            this.ValidateRequest();

            var claimsIdentity = CreateClaimsIdentityFromCredential(authRequest);

            if (claimsIdentity == null)
                throw new BadRequestException("Invalid credentials.");

            var claims = CreateClaims(claimsIdentity);
            var token = CreateJwt(claims);

            return new OkObjectResult(new Authenticated
            {
                Message = "Successfully authenticated.",
                Data = new Data
                {
                    AccessToken = token,
                    ExpiresIn = (int) _jwtConfig.ValidFor.TotalSeconds,
                    User = _database.FindUserByID(
                        claimsIdentity.FindFirst(AppConfig.AuthIdentifier).Value.ToInt()
                    )
                }
            });
        }

        /// <summary>
        ///     Deauthenticate a User.
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout", Name = "auth.logout")]
        public IActionResult Logout()
        {
            _cache.Remove(Request.GetBearerToken().CalculateMD5());

            return new OkObjectResult(new Success());
        }

        /// <summary>
        ///     Refresh the token.
        /// </summary>
        /// <returns></returns>
        [HttpPatch("refresh", Name = "auth.refresh")]
        public IActionResult Refresh()
        {
            var user = this.GetCurrentUser();
            var claimsIdentity = CreateClaimsIdentity(user);
            var claims = CreateClaims(claimsIdentity);
            var token = CreateJwt(claims);

            _cache.Remove(Request.GetBearerToken().CalculateMD5());

            return new OkObjectResult(new Refreshed
            {
                Message = "OK",
                Data = new Data
                {
                    AccessToken = token,
                    ExpiresIn = (int) _jwtConfig.ValidFor.TotalSeconds,
                    User = user
                }
            });
        }

        /// <summary>
        ///     Protected route for authenticated authRequest (any Role is OK).
        /// </summary>
        /// <returns></returns>
        [HttpGet("user", Name = "auth.user")]
        public IActionResult GetUser()
        {
            return new OkObjectResult(new Profile
            {
                Message = "OK",
                Data = this.GetCurrentUser()
            });
        }

        /// <summary>
        ///     Protected route for Role = Admin only.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "Admin")]
        [HttpGet("admin", Name = "auth.admin")]
        public IActionResult AdminPage()
        {
            return new OkObjectResult(new Profile
            {
                Message = "OK",
                Data = this.GetCurrentUser()
            });
        }

        /// <summary>
        ///     Validate credential.
        /// </summary>
        /// <param name="authRequest"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private bool TryIsValidCredential(AuthRequest authRequest, out User user)
        {
            if(authRequest == null) {
                user = null;

                return false;
            }

            var iUser = _database.FindUserByUsername(authRequest.Identity);

            user = iUser;

            return iUser != null && Verify(authRequest.Password, iUser.Password);
        }

        /// <summary>Retrieve claims through your claims provider</summary>
        /// <returns></returns>
        private ClaimsIdentity CreateClaimsIdentityFromCredential(AuthRequest authRequest)
        {
            return TryIsValidCredential(authRequest, out User iUser) ? CreateClaimsIdentity(iUser) : null;
        }

        /// <summary>
        ///     Create Claims Identity from User Model.
        /// </summary>
        /// <param name="iUser"></param>
        /// <returns>ClaimsIdentity.</returns>
        private static ClaimsIdentity CreateClaimsIdentity(User iUser)
        {
            return new ClaimsIdentity(
                new GenericIdentity(iUser.Username, "Token"),
                new[]
                {
                    new Claim(AppConfig.AuthIdentifier, iUser.Id.ToString()),
                    new Claim(AppConfig.AuthRoleIdentifierName, iUser.Role)
                });
        }

        /// <summary>
        ///     Create a bunch of Claim.
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <returns>List of claim.</returns>
        private IEnumerable<Claim> CreateClaims(ClaimsIdentity claimsIdentity)
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, claimsIdentity.FindFirst(AppConfig.AuthIdentifier).Value),
                new Claim(JwtRegisteredClaimNames.Jti, JwtConfig.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    _jwtConfig.IssuedAt.ToUnixEpochDate().ToString(),
                    ClaimValueTypes.Integer64),
                claimsIdentity.FindFirst(AppConfig.AuthRoleIdentifierName)
            };
        }

        /// <summary>
        ///     Create the JWT security token and encode it.
        /// </summary>
        /// <param name="claims"></param>
        /// <returns>Token with JWT format.</returns>
        private string CreateJwt(IEnumerable<Claim> claims)
        {
            var jwt = new JwtSecurityToken(
                _jwtConfig.Issuer,
                _jwtConfig.Audience,
                claims,
                _jwtConfig.NotBefore,
                _jwtConfig.Expiration,
                _jwtConfig.SigningCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            _cache.Set(token.CalculateMD5(), token,
                new MemoryCacheEntryOptions {SlidingExpiration = _jwtConfig.ValidFor});

            return token;
        }
    }
}