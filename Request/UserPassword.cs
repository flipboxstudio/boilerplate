using Newtonsoft.Json;
using App.Attributes;
using System.ComponentModel.DataAnnotations;

namespace App.Request
{
    public class UserPassword
    {
        [Required]
        [JsonProperty("old_password")]
        public string OldPassword { get; set; }

        [Required]
        [JsonProperty("new_password")]
        public string NewPassword { get; set; }

        [Required]
        [Compare(nameof(NewPassword))]
        [JsonProperty("confirm_password")]
        public string ConfirmPassword { get; set; }
    }
}