#region using

using System.ComponentModel.DataAnnotations;

#endregion

namespace App.Requests
{
    public class RegistrationRequest
    {
        /// <summary>
        ///     Get or set email.
        /// </summary>
        /// <returns></returns>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        ///     Get or set password.
        /// </summary>
        /// <returns></returns>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}