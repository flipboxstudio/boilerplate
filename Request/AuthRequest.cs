using System.ComponentModel.DataAnnotations;

namespace App.Request
{
    public class AuthRequest
    {
        [Required]
        public string Identity { get; set; }

        [Required]
        public string Password { get; set; }
    }
}