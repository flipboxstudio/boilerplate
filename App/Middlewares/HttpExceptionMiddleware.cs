#region using

using System.Threading.Tasks;
using App.Exceptions;
using Microsoft.AspNetCore.Http;

#endregion

namespace App.Middlewares
{
    internal class HttpExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        /// <summary>
        ///     Class constructor.
        /// </summary>
        /// <param name="requestDelegate"></param>
        public HttpExceptionMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        /// <summary>
        ///     Invoke the middleware.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _requestDelegate.Invoke(httpContext);
            }
            catch (HttpException httpException)
            {
                httpContext.Response.StatusCode = httpException.HttpStatusCode;
                httpContext.Response.ContentType = httpException.ContentType;

                await httpContext.Response.WriteAsync(httpException.Content ?? httpException.Message);
            }
        }
    }
}