#pragma warning disable 168

using System;
using System.IdentityModel.Tokens.Jwt;
using Boilerplate.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace Boilerplate.Services
{
    public class TokenValidator : JwtSecurityTokenHandler
    {
        public IMemoryCache Cache { private get; set; }

        public override bool CanReadToken(string securityToken)
        {
            var cacheIdentifier = securityToken.CalculateMD5();

            if (!Cache.TryGetValue(cacheIdentifier, out string cached))
                throw new UnauthorizedException("Token Expired.");

            return base.CanReadToken(securityToken);
        }
    }
}