using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Boilerplate.Model
{
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