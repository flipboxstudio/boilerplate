using App;
using App.Exceptions;
using App.Model;
using App.Services;
using System;
using System.Linq;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerExtension
    {
        public static void ValidateRequest(this Controller controller)
        {
            if (!controller.ModelState.IsValid)
                throw new ValidationException("Some attribute(s) fail to pass validation.", controller.ModelState);
        }

        public static User GetCurrentUser(this Controller controller)
        {
            var database = (Database) controller.HttpContext.RequestServices.GetService(typeof(Database));

            return database.FindUserByID(controller.User.Claims.First().Value.ToInt());
        }
    }
}