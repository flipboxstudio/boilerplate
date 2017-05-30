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
            return request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        }
    }
}