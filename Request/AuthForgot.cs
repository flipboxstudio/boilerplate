using App.Attributes;
using static BCrypt.Net.BCrypt;
using System.ComponentModel.DataAnnotations;

namespace App.Request
{
    public class AuthForgot
    {
        [Required]
        [Exists("Users", "email", ErrorMessage = "Email does not exist in our record.")]
        public string Email { get; set; }
    }
}