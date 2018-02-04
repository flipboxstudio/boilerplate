#region using

using App.Responses;
using Microsoft.AspNetCore.Mvc;

#endregion

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View(new SpaResponse
        {
            UrlPath = Request.Path
        });
    }
}