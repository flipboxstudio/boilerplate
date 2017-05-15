using App.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace App.Response.v1 {
    public class Search {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public IEnumerable<GeoNames> Data { get; set; }
    }
}