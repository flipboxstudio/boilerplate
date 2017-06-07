using System;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace App.Services
{
    public class ProtobufInputFormatter : InputFormatter
    {
        /// <summary>
        /// Header value.
        /// </summary>
        /// <returns></returns>
        static MediaTypeHeaderValue protoMediaType = MediaTypeHeaderValue.Parse("application/x-protobuf");

        /// <summary>
        /// Determine if current request is sent using protobuf.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanRead(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            MediaTypeHeaderValue requestContentType = null;

            MediaTypeHeaderValue.TryParse(request.ContentType, out requestContentType);

            return (requestContentType == null) ? false : requestContentType.IsSubsetOf(protoMediaType);
        }

        /// <summary>
        /// Read from protobuf.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            try
            {
                var request = context.HttpContext.Request;
                var obj = (IMessage) Activator.CreateInstance(context.ModelType);

                obj.MergeFrom(request.Body);

                return InputFormatterResult.SuccessAsync(obj);
            }
            catch (Exception)
            {
                return InputFormatterResult.FailureAsync();
            }
        }
    }
}