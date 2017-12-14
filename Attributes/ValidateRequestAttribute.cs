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
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                base.OnActionExecuting(context);

                return;
            }

            var errorList = context.ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(error => error.ErrorMessage).ToArray()
            );

            context.Result = new BadRequestObjectResult(new
            {
                Message = "Failed.",
                Errors = errorList
            });
        }
    }
}