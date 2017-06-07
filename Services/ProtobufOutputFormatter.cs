using System;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Reflection;
using System.Linq;
using Microsoft.Extensions.Primitives;

namespace App.Services
{
    public class ProtobufOutputFormatter : OutputFormatter
    {
        /// <summary>
        /// Determine if current request wants protobuf.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool CanWriteResult(OutputFormatterCanWriteContext context)
        {
            if (context.Object == null || !context.ContentType.Equals(new StringSegment("application/x-protobuf")))
            {
                return false;
            }

            // Check whether the given object is a proto-generated object
            return context.ObjectType.GetTypeInfo()
                .ImplementedInterfaces
                .Where(type => type.GetTypeInfo().IsGenericType)
                .Any(type => type.GetGenericTypeDefinition() == typeof(IMessage<>));
        }

        /// <summary>
        /// Write protobuf response to client.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;

            // Proto-encode
            var protoObj = context.Object as IMessage;
            var serialized = protoObj.ToByteArray();

            return response.Body.WriteAsync(serialized, 0, serialized.Length);
        }
    }
}