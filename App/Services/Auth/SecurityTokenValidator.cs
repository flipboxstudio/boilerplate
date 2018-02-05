#region using

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Services.Db.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

#endregion

namespace App.Services.Auth
{
    public class SecurityTokenInvalidUserException : SecurityTokenException
    {
        public SecurityTokenInvalidUserException(string message) : base(message)
        {
        }
    }

    public class SecurityTokenValidator : JwtSecurityTokenHandler
    {
        /// <summary>
        ///     User Manager.
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <inheritdoc />
        /// <summary>
        ///     Class Constructor.
        /// </summary>
        /// <param name="userManager"></param>
        public SecurityTokenValidator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <inheritdoc />
        public override ClaimsPrincipal ValidateToken(string plainToken,
            TokenValidationParameters tokenValidationParameter, out SecurityToken securityToken)
        {
            var claimsPrincipal = base.ValidateToken(plainToken, tokenValidationParameter, out securityToken);

            if (Task.Run<ApplicationUser>(
                    async () => await _userManager.GetUserAsync(claimsPrincipal)
                ).Result == null
            )
            {
                throw LogHelper.LogExceptionMessage(
                    new SecurityTokenInvalidUserException("IDX10800: Unable to obtain user."));
            }

            return claimsPrincipal;
        }
    }
}