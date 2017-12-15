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
        public HttpException(int httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Class constructor.
        /// </summary>
        /// <param name="httpStatusCode"></param>
        public HttpException(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = (int) httpStatusCode;
        }

        /// <summary>
        ///     HTTP status code.
        /// </summary>
        /// <returns></returns>
        public int HttpStatusCode { get; }

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