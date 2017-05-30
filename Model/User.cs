using System;
using Newtonsoft.Json;
using App.Request;

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
    }
}