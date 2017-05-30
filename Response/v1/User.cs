using App.Model;
using Newtonsoft.Json;

namespace App.Response.v1
{
    public class UserAvatar
    {
        public string Message { get; set; }

        public User Data { get; set; }
    }

    public class UserProfile
    {
        public string Message { get; set; }

        public User Data { get; set; }
    }

    public class UserPassword
    {
        public string Message { get; set; }

        public User Data { get; set; }
    }
}