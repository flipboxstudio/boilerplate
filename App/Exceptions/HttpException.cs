#region using

using System;
using System.Net;

#endregion

namespace App.Exceptions
{
    public class HttpException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        ///     Class constructor.
        /// </summary>
        /// <param name="httpStatusCode"></param>
        public HttpException(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        ///     HTTP status code.
        /// </summary>
        /// <returns></returns>
        public HttpStatusCode HttpStatusCode { get; }

        /// <summary>
        ///     Response content.
        /// </summary>
        /// <returns></returns>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        ///     Response content type.
        /// </summary>
        /// <returns></returns>
        public string ContentType { get; set; } = "application/json";
    }
}