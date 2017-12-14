using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class AppController : Controller
    {
        /// <summary>
        /// Default route.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public object Index()
        {
            return new {
                Message = "OK"
            };
        }
    }
}