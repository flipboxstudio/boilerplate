using Microsoft.Extensions.Primitives;

namespace Microsoft.AspNetCore.Http
{
    public static class RequestExtension
    {
        /// <summary>
        /// Get bearer token from current request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetBearerToken(this HttpRequest request)
        {
            var header = new StringValues();

            if (! request.Headers.TryGetValue("Authorization", out header)) {
                return "";
            }

            return header.ToString().Replace("Bearer ", "");
        }
    }
}
