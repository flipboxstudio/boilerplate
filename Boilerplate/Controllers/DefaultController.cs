using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Controllers
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
        public IActionResult Get()
        {
            return Ok(new
            {
                message = "You have arrived.",
                data = new Dictionary<string, string>
                {
                    {"root", Url.AbsoluteRouteUrl("root")},
                    {"root.v1", Url.AbsoluteContent("v1")}
                }
            });
        }
    }
}