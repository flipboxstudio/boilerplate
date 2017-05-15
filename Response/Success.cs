using Newtonsoft.Json;

namespace App.Response {
    public class Success {
        [JsonProperty("message")]
        public string Message { get; set; } = "Ok.";

        [JsonProperty("data")]
        public object Data { get; set; } = new { };
    }
}