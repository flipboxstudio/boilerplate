using Newtonsoft.Json;
using App.Attributes;
using static BCrypt.Net.BCrypt;
using System.ComponentModel.DataAnnotations;

namespace App.Request
{
    public class UserProfile
    {
        [Required]
        [JsonProperty("email")]
        [Unique("Users", "email", false, ErrorMessage = "Email has been taken.")]
        public string Email { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("nick_name")]
        public string NickName { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}