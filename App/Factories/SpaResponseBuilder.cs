#region using

using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using App.Responses;
using App.Services.Db.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

#endregion

namespace App.Factories
{
    public class SpaResponseBuilder
    {
        private HttpContext _httpContext;

        private UserManager<ApplicationUser> _userManager;

        private SpaResponse _response;

        public SpaResponseBuilder(
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager
        )
        {
            _httpContext = httpContextAccessor.HttpContext;
            _userManager = userManager;
            _response = new SpaResponse();
        }

        public SpaResponse Make()
        {
            PrepareResponse();

            return _response;
        }

        private void PrepareResponse()
        {
            PrepareAuthResponse();
        }

        private void PrepareAuthResponse()
        {
            _response.Auth.User = GetAuthenticatedUser();
        }

        private ApplicationUser GetAuthenticatedUser()
        {
            var claimsPrincipal = _httpContext.User;

            return Task.Run<ApplicationUser>(
                async () => await _userManager.GetUserAsync(claimsPrincipal)
            ).Result;
        }
    }
}