#region using

using Microsoft.AspNetCore.Mvc;

#endregion

namespace App.Controllers.Api.v1
{
    [Route("api/v1/[action]")]
    public class ApiV1Controller : Controller
    {
        /// <summary>
        ///     Default route.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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