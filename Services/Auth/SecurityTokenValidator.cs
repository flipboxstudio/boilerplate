using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Services.Db.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace App.Services.Auth
{
    public class SecurityTokenInvalidUserException : SecurityTokenInvalidAudienceException
    {
        public SecurityTokenInvalidUserException(string message) : base(message)
        {
        }
    }

    public class SecurityTokenInvalidClaimsException : SecurityTokenInvalidAudienceException
    {
        public SecurityTokenInvalidClaimsException(string message) : base(message)
        {
        }
    }

    public class SecurityTokenValidator : JwtSecurityTokenHandler
    {
        /// <summary>
        /// User Manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        /// Class Constructor.
        /// </summary>
        /// <param name="userManager"></param>
        public SecurityTokenValidator(UserManager<ApplicationUser> userManager) => _userManager = userManager;

        /// <inheritdoc />
        public override ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var claimsPrincipal = base.ValidateToken(securityToken, validationParameters, out validatedToken);
            var user = Task.Run(async () => await _userManager.GetUserAsync(claimsPrincipal)).Result;

            if (user == null)
            {
                throw LogHelper.LogExceptionMessage(new SecurityTokenInvalidUserException("IDX10800: Unable to obtain user."));
            }

            return claimsPrincipal;
        }
    }
}