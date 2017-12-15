#region using

using Microsoft.AspNetCore.Mvc;

#endregion

namespace App.Controllers
{
    public class AppController : Controller
    {
        /// <summary>
        ///     Default route.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public object Index()
        {
            return new
            {
                Message = "OK.",
                Status = 1
            };
        }
    }
}