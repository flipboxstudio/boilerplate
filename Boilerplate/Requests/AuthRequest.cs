using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Boilerplate.Requests
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class AuthRequest
    {
        [Required]
        public string Identity { get; set; }

        [Required]
        public string Password { get; set; }
    }
}