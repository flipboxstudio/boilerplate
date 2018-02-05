#region using

using App.Factories;
using App.Responses;
using Microsoft.AspNetCore.Mvc;

#endregion

public class HomeController : Controller
{
    private SpaResponseBuilder _spaResponseBuilder;

    public HomeController(SpaResponseBuilder response)
    {
        _spaResponseBuilder = response;
    }

    public IActionResult Index()
    {
        return View(_spaResponseBuilder.Make());
    }
}
