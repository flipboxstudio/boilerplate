using App.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using App.Response.v1;

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
        public Hello Get()
        {
            return new Hello
            {
                Message = "You have arrived."
            };
        }
    }
}