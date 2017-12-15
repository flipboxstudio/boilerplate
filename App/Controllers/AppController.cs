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
        public object Index() => new
        {
            Message = "OK.",
            Status = 1,
        };
    }
}