using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Model
{
    [Table("Users")]
    public class User
    {
        [Column("id")]
        [JsonProperty("user_id")]
        public int Id { get; set; }

        [Column("username")]
        [JsonProperty("username")]
        public string Username { get; set; }

        [Column("role")]
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonIgnore]
        [Column("password")]
        public string Password { get; set; }
    }
}