using App.Response.v1;
using App.Request;
using App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers.v1
{
    [Route("v1/[controller]", Name = "geonames")]
    public class GeoNamesController : Controller
    {
        private readonly Database _database;

        public GeoNamesController(Database database)
        {
            _database = database;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get([FromQuery] Queryable request)
        {
            return new OkObjectResult(new Search {
                Message = "Ok.",
                Data = _database.SearchGeoNames(request.Query)
            });
        }
    }
}