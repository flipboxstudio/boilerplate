using System;
using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace App.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message = "Bad Request.", ModelStateDictionary data = null)
        {
            Message = message;
            Data = FormatModelState(data);
        }

        public override string Message { get; }

        public override IDictionary Data { get; }

        /// <returns>List of failed validation(s).</returns>
        private static IDictionary FormatModelState(ModelStateDictionary modalState)
        {
            return modalState.ToDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => keyValuePair.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
        }
    }
}