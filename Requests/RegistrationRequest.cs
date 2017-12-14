using System.ComponentModel.DataAnnotations;

namespace App.Requests
{
    public class RegistrationRequest
    {
        /// <summary>
        /// Get or set email.
        /// </summary>
        /// <returns></returns>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Get or set password.
        /// </summary>
        /// <returns></returns>
        [Required]
        [StringLength(maximumLength: 100, MinimumLength = 6)]
        public string Password { get; set; }
    }
}