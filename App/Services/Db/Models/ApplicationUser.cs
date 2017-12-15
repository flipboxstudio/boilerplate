#region using

using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

#endregion

namespace App.Services.Db.Models
{
    public class ApplicationUser : IdentityUser<string>
    {
        [JsonIgnore]
        public override string PasswordHash { get; set; }
    }
}