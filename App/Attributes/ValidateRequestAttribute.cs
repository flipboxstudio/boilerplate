using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.Attributes
{
    public class ValidateRequestAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Validate request before execute the controller.
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
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(error => error.ErrorMessage).ToArray()
                )
            });
        }
    }
}