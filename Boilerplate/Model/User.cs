using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Boilerplate.Model
{
    [Table("Users")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class User
    {
        [Key]
        [JsonProperty("UserID")]
        public int UserID { get; set; }

        [Required]
        [StringLength(16)]
        [JsonProperty("Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(8)]
        [JsonProperty("Role")]
        public string Role { get; set; }

        [Required]
        [JsonIgnore]
        [StringLength(64)]
        public string Password { get; set; }
    }
}