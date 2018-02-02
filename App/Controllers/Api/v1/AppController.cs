#region using

using Microsoft.AspNetCore.Mvc;

#endregion

namespace App.Controllers.Api.v1
{
    [Route("api/v1")]
    public class AppController : Controller
    {
        /// <summary>
        ///     Default route.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public object Get()
        {
            return new
            {
                Message = "OK.",
                Status = 1
            };
        }
    }
}