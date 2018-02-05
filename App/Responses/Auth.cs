#region using

using App.Services.Db.Models;

#endregion

namespace App.Responses
{
    public class Auth
    {
        public string Token { get; set; }

        public ApplicationUser User { get; set; }
    }
}