using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Boilerplate.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Boilerplate.Middlewares
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ErrorHandlerMiddleware
    {
        private readonly IHostingEnvironment _env;
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(IHostingEnvironment env, RequestDelegate next)
        {
            _next = next;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode.Equals(400)) throw new BadRequestException("Bad Request.");
                if (context.Response.StatusCode.Equals(401)) throw new UnauthorizedException("Unauthorized.");
                if (context.Response.StatusCode.Equals(403)) throw new ForbiddenException("Forbidden.");
                if (context.Response.StatusCode.Equals(404)) throw new NotFoundException("Not Found.");
                if (context.Response.StatusCode.Equals(415))
                    throw new UnsupportedMediaTypeException("Unsupported Media Type.");
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            if (exception is ValidationException || exception is BadRequestException) code = HttpStatusCode.BadRequest;
            else if (exception is UnauthorizedException) code = HttpStatusCode.Unauthorized;
            else if (exception is ForbiddenException) code = HttpStatusCode.Forbidden;
            else if (exception is NotFoundException) code = HttpStatusCode.NotFound;
            else if (exception is UnsupportedMediaTypeException) code = HttpStatusCode.UnsupportedMediaType;

            var result = JsonConvert.SerializeObject(BuildExceptionData(exception));

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;

            return context.Response.WriteAsync(result);
        }

        private Dictionary<string, object> BuildExceptionData(Exception exception)
        {
            var data = new Dictionary<string, object>
            {
                {"message", exception.Message},
                {"error", exception.Data}
            };

            if (!_env.IsDevelopment()) return data;

            data.Add("$trace", FormatExceptionStackTrace(exception));

            return data;
        }

        private static List<string> FormatExceptionStackTrace(Exception exception)
        {
            return exception.StackTrace
                .Split(new[] {"\r\n", "\n"}, StringSplitOptions.None)
                .Select(p => p.Trim())
                .ToList();
        }
    }
}