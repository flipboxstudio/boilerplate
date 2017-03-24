using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Controllers.v1
{
    [Route("v1", Name = "root.v1")]
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
                data = new {version = 1}
            });
        }
    }
}