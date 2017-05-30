using App.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace App.Controllers
{
    [Route("", Name = "root")]
    public class DefaultController : Controller
    {
        /// <summary>
        ///     Got root?
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public Success Get()
        {
            return new Success
            {
                Message = "You have arrived."
            };
        }
    }
}