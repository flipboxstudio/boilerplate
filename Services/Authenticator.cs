using System.Security.Claims;
using App.Options;
using App.Request;
using App.Model;
using Microsoft.Extensions.Options;
using static BCrypt.Net.BCrypt;
using System.Security.Principal;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System;
using App.Exceptions;
using App.Response.v1;

namespace App.Services
{
    public class Authenticator
    {
        /// <summary>
        /// Database instance.
        /// </summary>
        private readonly Database _database;

        /// <summary>
        /// JWT configuration.
        /// </summary>
        private readonly JwtConfig _jwtConfig;

        /// <summary>
        /// User instance.
        /// </summary>
        /// <returns></returns>
        public User User { get; private set; }

        /// <summary>
        /// Class contructor.
        /// </summary>
        /// <param name="database"></param>
        /// <param name="jwtConfig"></param>
        public Authenticator(Database database, IOptions<JwtConfig> jwtConfig)
        {
            _database = database;
            _jwtConfig = jwtConfig.Value;
        }

        /// <summary>
        /// Retrieve claims through your claims provider.
        /// </summary>
        /// <param name="authRequest"></param>
        /// <returns></returns>
        public AuthData Authenticate(AuthRequest authRequest)
        {
            var claimsIdentity = TryIsValidCredential(authRequest, out User user)
                ? CreateClaimsIdentity(user)
                : null;

            if (claimsIdentity == null)
                throw new BadRequestException("Invalid credentials.");

            var token = GenerateToken(claimsIdentity);

            return new AuthData {
                AccessToken = token,
                ExpiresIn = (int) _jwtConfig.ValidFor.TotalSeconds,
                User = user
            };
        }

        /// <summary>
        ///     Create a bunch of Claim.
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <returns>List of claim.</returns>
        public IEnumerable<Claim> CreateClaims(ClaimsIdentity claimsIdentity)
        {
            return new[]
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    claimsIdentity.FindFirst(AppConfig.AuthIdentifier).Value
                ),
                new Claim(JwtRegisteredClaimNames.Jti, JwtConfig.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    _jwtConfig.IssuedAt.ToUnixEpochDate().ToString(),
                    ClaimValueTypes.Integer64)
            };
        }

        /// <summary>
        ///     Create the JWT security token and encode it.
        /// </summary>
        /// <param name="claims"></param>
        /// <returns>Token with JWT format.</returns>
        public string GenerateToken(ClaimsIdentity claimsIdentity)
        {
            var jwt = new JwtSecurityToken(
                _jwtConfig.Issuer,
                _jwtConfig.Audience,
                CreateClaims(claimsIdentity),
                _jwtConfig.NotBefore,
                _jwtConfig.Expiration,
                _jwtConfig.SigningCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }

        /// <summary>
        ///     Create Claims Identity from User Model.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>ClaimsIdentity.</returns>
        public ClaimsIdentity CreateClaimsIdentity(User user)
        {
            return new ClaimsIdentity(
                new GenericIdentity(user.Email, "Token"),
                new[]
                {
                    new Claim(AppConfig.AuthIdentifier, user.Id.ToString()),
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

            User = user = _database.FindUserByEmail(authRequest.Email);

            return user != null && Verify(authRequest.Password, user.Password);
        }
    }
}