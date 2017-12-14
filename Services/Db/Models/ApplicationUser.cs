using System;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace App.Services.Db.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [JsonIgnore]
        public override string PasswordHash { get; set; }
    }
}