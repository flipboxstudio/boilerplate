using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using App.Services.Db.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace App.Services.Auth
{
    public class TokenGenerator
    {
        private readonly AppSettings appSettings;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="settings"></param>
        public TokenGenerator(IOptions<AppSettings> settings) => appSettings = settings.Value;

        /// <summary>
        /// Generate JWT.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string GenerateToken(ApplicationUser user)
        {
            var claims = GenerateClaims(user);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(appSettings.Jwt.ExpiryDays);
            var token = new JwtSecurityToken(
                issuer: appSettings.Jwt.Issuer,
                audience: appSettings.Jwt.Issuer,
                claims: claims,
                expires: expires,
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static List<Claim> GenerateClaims(ApplicationUser user) => new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),

                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };
    }
}