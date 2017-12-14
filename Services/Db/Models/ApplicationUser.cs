using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace App.Services.Db.Models
{
    public class ApplicationUser : IdentityUser
    {
        [JsonIgnore]
        public override string PasswordHash { get; set; }

        [JsonIgnore]
        public override string SecurityStamp { get; set; }

        [JsonIgnore]
        public override string ConcurrencyStamp { get; set; }
    }
}