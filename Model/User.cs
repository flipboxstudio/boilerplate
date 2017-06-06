using System;
using Newtonsoft.Json;
using App.Request;
using static BCrypt.Net.BCrypt;

namespace App.Model
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string NickName { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public string Phone { get; set; }

        public string Avatar { get; set; }

        public bool NeedToChangePassword { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Create new instance from request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static User CreateFromRequest(AuthRegister request)
        {
            return new User {
                Email = request.Email,
                FullName = request.FullName,
                NickName = request.NickName,
                Password = request.Password,
                Phone = request.Phone,
                Avatar = string.Format("https://www.gravatar.com/avatar/{0}", request.Email.CalculateMD5())
            };
        }

        /// <summary>
        /// Update user profile from request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public User UpdateFromRequest(UserProfile request)
        {
            FullName = request.FullName;
            NickName = request.NickName;
            Email = request.Email;
            Phone = request.FullName;

            return this;
        }

        /// <summary>
        /// Forgot a user password.
        /// </summary>
        /// <param name="plainPassword"></param>
        /// <returns></returns>
        public User Forgot(string plainPassword)
        {
            Password = HashPassword(plainPassword);
            NeedToChangePassword = true;

            return this;
        }

        /// <summary>
        /// Change a user password.
        /// </summary>
        /// <param name="plainPassword"></param>
        /// <returns></returns>
        public User ChangePassword(string plainPassword)
        {
            Password = HashPassword(plainPassword);
            NeedToChangePassword = false;

            return this;
        }
    }
}
