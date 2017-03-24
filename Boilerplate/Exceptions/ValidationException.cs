using System;
using System.Collections;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Boilerplate.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message = "", ModelStateDictionary data = null)
        {
            Message = message;
            Data = FormatModelState(data);
        }

        public override string Message { get; }

        public override IDictionary Data { get; }

        /// <returns>List of failed validation(s).</returns>
        private static IDictionary FormatModelState(ModelStateDictionary modalState)
        {
            return modalState == null
                ? null
                : (
                    from state in modalState
                    from error in state.Value.Errors
                    select error.Exception?.Message ?? error.ErrorMessage ?? ""
                ).ToDictionary(key => key, value => value);
        }
    }
}