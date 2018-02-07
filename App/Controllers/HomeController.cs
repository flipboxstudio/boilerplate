#region using

using App.Factories;
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
        var data = _spaResponseBuilder.Make();

        return View(data);
    }
}
