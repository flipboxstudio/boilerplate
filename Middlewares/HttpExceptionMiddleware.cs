using System.Threading.Tasks;
using App.Exceptions;
using Microsoft.AspNetCore.Http;

namespace App.Middlewares
{
    internal class HttpExceptionMiddleware
    {
        private readonly RequestDelegate next;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="next"></param>
        public HttpExceptionMiddleware(RequestDelegate next) => this.next = next;

        /// <summary>
        /// Invoke the middleware.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (HttpException httpException)
            {
                context.Response.StatusCode = httpException.HttpStatusCode;
                context.Response.ContentType = httpException.ContentType;

                await context.Response.WriteAsync(httpException.Content ?? httpException.Message);
            }
        }
    }
}