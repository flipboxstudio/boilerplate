#region using

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

#endregion

namespace App.Attributes
{
    public class ValidateRequestAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        /// <summary>
        ///     Validate request before execute the controller.
        /// </summary>
        /// <param name="actionExecutingContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionExecutingContext)
        {
            if (actionExecutingContext.ModelState.IsValid)
            {
                base.OnActionExecuting(actionExecutingContext);

                return;
            }

            actionExecutingContext.Result = new BadRequestObjectResult(new
            {
                Message = "Failed.",
                Errors = actionExecutingContext.ModelState.ToDictionary(
                    keyValuePair => keyValuePair.Key,
                    keyValuePair => keyValuePair.Value.Errors.Select(
                        modelError => modelError.ErrorMessage
                    ).ToArray()
                )
            });
        }
    }
}