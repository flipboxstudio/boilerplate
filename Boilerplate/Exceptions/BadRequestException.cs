using System;
using System.Collections;
using System.Collections.Generic;

namespace Boilerplate.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message = "", IDictionary data = null)
        {
            Message = message;
            Data = data ?? new Dictionary<string, string>();
        }

        public override string Message { get; }

        public override IDictionary Data { get; }
    }
}