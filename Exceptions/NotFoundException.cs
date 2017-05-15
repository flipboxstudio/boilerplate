using System;
using System.Collections;
using System.Collections.Generic;

namespace App.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "Not Found.", IDictionary data = null)
        {
            Message = message;
            Data = data ?? new Dictionary<string, string>();
        }

        public override string Message { get; }

        public override IDictionary Data { get; }
    }
}