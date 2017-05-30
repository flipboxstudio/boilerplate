using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace App.Request
{
    public class UserAvatar
    {
        [Required]
        public IFormFile Avatar { get; set; }
    }
}