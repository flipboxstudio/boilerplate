// ReSharper disable CheckNamespace

namespace Microsoft.AspNetCore.Http
{
    public static class RequestExtension
    {
        public static string GetBearerToken(this HttpRequest request)
        {
            return request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }
    }
}