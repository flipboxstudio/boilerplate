using Newtonsoft.Json;
using App.Attributes;
using static BCrypt.Net.BCrypt;
using System.ComponentModel.DataAnnotations;

namespace App.Request
{
    public class AuthRegister
    {
        [Required]
        [Unique("Users", "email", ErrorMessage = "Email has been taken.")]
        public string Email { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("nick_name")]
        public string NickName { get; set; }

        private string _password;

        [Required]
        public string Password
        { 
            get
            {
                return _password;
            }

            set
            {
                _password = HashPassword(value);
            }
        }

        public string Phone { get; set; }
    }
}