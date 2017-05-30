using System;
using System.Collections;
using System.Collections.Generic;

namespace App.Exceptions
{
    public class ForbiddenException : Exception
    {
        public override string Message { get; }

        public override IDictionary Data { get; }

        public ForbiddenException(string message = "Forbidden.", IDictionary data = null)
        {
            Message = message;
            Data = data ?? new Dictionary<string, string>();
        }
    }
}