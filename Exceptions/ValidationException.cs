using System;
using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace App.Exceptions
{
    public class ValidationException : Exception
    {
        public override string Message { get; }

        public override IDictionary Data { get; }

        /// <summary>
        /// Constructor, here you can add your model state so we can print any failed validation.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        public ValidationException(string message = "Bad Request.", ModelStateDictionary data = null)
        {
            Message = message;
            Data = FormatModelState(data);
        }

        /// <summary>
        /// Format Model State to our standard error format.
        /// </summary>
        /// <param name="modalState"></param>
        /// <returns></returns>
        private static IDictionary FormatModelState(ModelStateDictionary modalState)
        {
            return modalState.ToDictionary(
                keyValuePair => keyValuePair.Key,
                keyValuePair => keyValuePair.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
        }
    }
}