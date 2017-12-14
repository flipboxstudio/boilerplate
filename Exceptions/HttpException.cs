using System;
using System.Net;

namespace App.Exceptions
{
    public class HttpException : Exception
    {
        private readonly int httpStatusCode;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="httpStatusCode"></param>
        public HttpException(int httpStatusCode)
        {
            this.httpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="httpStatusCode"></param>
        public HttpException(HttpStatusCode httpStatusCode)
        {
            this.httpStatusCode = (int) httpStatusCode;
        }

        /// <summary>
        /// HTTP status code.
        /// </summary>
        /// <returns></returns>
        public int HttpStatusCode { get { return this.httpStatusCode; } }

        /// <summary>
        /// Response content.
        /// </summary>
        /// <returns></returns>
        public string Content { get; set; }

        /// <summary>
        /// Response content type.
        /// </summary>
        /// <returns></returns>
        public string ContentType { get; set; } = "application/json";
    }
}