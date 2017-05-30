using Newtonsoft.Json;

namespace App.Response
{
    public class Success
    {
        public string Message { get; set; } = "OK";

        public object Data { get; set; } = new { };
    }
}