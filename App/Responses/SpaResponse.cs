#region using

#endregion

namespace App.Responses
{
    public class SpaResponse
    {
        public string UrlPath { get; set; }

        public Auth Auth { get; set; } = new Auth();
    }
}