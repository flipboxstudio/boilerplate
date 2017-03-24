// ReSharper disable CheckNamespace

using System;
using System.Linq;
using Boilerplate;
using Boilerplate.Exceptions;
using Boilerplate.Model;
using Boilerplate.Services;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerExtension
    {
        public static void ValidateRequest(this Controller controller)
        {
            if (!controller.ModelState.IsValid)
                throw new ValidationException("Some attribute(s) fail to pass validation", controller.ModelState);
        }

        public static User GetUserPrincipal(this Controller controller)
        {
            var database = (Database) controller.HttpContext.RequestServices.GetService(typeof(Database));

            return database.FindUserByID(controller.User.Claims.First().Value.ToInt());
        }
    }
}