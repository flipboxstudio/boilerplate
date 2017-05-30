using System;
using System.Collections;
using System.Collections.Generic;

namespace App.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public override string Message { get; }

        public override IDictionary Data { get; }

        public UnauthorizedException(string message = "Unauthorized.", IDictionary data = null)
        {
            Message = message;
            Data = data ?? new Dictionary<string, string>();
        }
    }
}