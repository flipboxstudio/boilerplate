using System;
using System.Collections;
using System.Collections.Generic;

namespace App.Exceptions
{
    public class UnsupportedMediaTypeException : Exception
    {
        public override string Message { get; }

        public override IDictionary Data { get; }

        public UnsupportedMediaTypeException(string message = "Unsupported Media Type.", IDictionary data = null)
        {
            Message = message;
            Data = data ?? new Dictionary<string, string>();
        }
    }
}