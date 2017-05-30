using App.Model;
using Newtonsoft.Json;

namespace App.Response.v1
{
    public class Authenticated
    {
        public string Message { get; set; }

        public AuthData Data { get; set; }
    }

    public class Registered
    {
        public string Message { get; set; }

        public User Data { get; set; }
    }

    public class Forgot
    {
        public string Message { get; set; }

        public User Data { get; set; }
    }

    public class Profile
    {
        public string Message { get; set; }

        public User Data { get; set; }
    }

    public class AuthData
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
}