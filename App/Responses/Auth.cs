#region using

using App.Services.Db.Models;

#endregion

namespace App.Responses
{
    public class Auth
    {
        public ApplicationUser User { get; set; }
    }
}