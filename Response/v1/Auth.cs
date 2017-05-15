using App.Model;
using Newtonsoft.Json;

namespace App.Response.v1 {
    public class Authenticated {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Refreshed {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public class Profile {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public User Data { get; set; }
    }

    public class Data {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
}