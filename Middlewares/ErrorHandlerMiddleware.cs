using App.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace App.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        /// <summary>
        /// Hosting environment.
        /// </summary>
        private readonly IHostingEnvironment _env;

        /// <summary>
        /// Request chainable object.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="next"></param>
        public ErrorHandlerMiddleware(IHostingEnvironment env, RequestDelegate next)
        {
            _next = next;
            _env = env;
        }

        /// <summary>
        /// Here we catch any exception thrown by application.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode.Equals(401))
                    throw new UnauthorizedException("Unauthorized.");
                else if (context.Response.StatusCode.Equals(403))
                    throw new ForbiddenException("Forbidden.");
                else if (context.Response.StatusCode.Equals(404))
                    throw new NotFoundException("Not Found.");
                else if (context.Response.StatusCode.Equals(415))
                    throw new UnsupportedMediaTypeException("Unsupported Media Type.");
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        /// <summary>
        /// Handle any exception thrown by application.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            if (exception is BadRequestException) code = HttpStatusCode.BadRequest;
            else if (exception is ValidationException) code = HttpStatusCode.PreconditionFailed;
            else if (exception is UnauthorizedException) code = HttpStatusCode.Unauthorized;
            else if (exception is ForbiddenException) code = HttpStatusCode.Forbidden;
            else if (exception is NotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is UnsupportedMediaTypeException) code = HttpStatusCode.UnsupportedMediaType;

            var result = JsonConvert.SerializeObject(BuildExceptionData(exception));

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;

            return context.Response.WriteAsync(result);
        }

        /// <summary>
        /// Format exception data. Print stack trace if it's on development server.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private Dictionary<string, object> BuildExceptionData(Exception exception)
        {
            var data = new Dictionary<string, object>
            {
                { "message", exception.Message },
                { "error", exception.Data }
            };

            if (!_env.IsDevelopment()) return data;

            data.Add(".trace", FormatExceptionStackTrace(exception));

            return data;
        }

        /// <summary>
        /// Format the stack trace to make it more readable.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static List<string> FormatExceptionStackTrace(Exception exception)
        {
            return exception.StackTrace
                .Split(new[] {"\r\n", "\n"}, StringSplitOptions.None)
                .Select(traceLine => traceLine.Trim())
                .ToList();
        }
    }
}
