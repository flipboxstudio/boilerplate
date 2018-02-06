#region using

using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using App.Factories;
using App.Responses;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.AspNetCore.SpaServices.Prerendering;

#endregion

public class HomeController : Controller
{
    private SpaResponseBuilder _spaResponseBuilder;

    public HomeController(SpaResponseBuilder response)
    {
        _spaResponseBuilder = response;
    }

    public async Task<IActionResult> Index([FromServices] INodeServices nodeServices, [FromServices] IHostingEnvironment hostEnv)
    {
        var applicationBasePath = hostEnv.ContentRootPath;
        var requestFeature = Request.HttpContext.Features.Get<IHttpRequestFeature>();
        var unencodedPathAndQuery = requestFeature.RawTarget;
        var unencodedAbsoluteUrl = $"{Request.Scheme}://{Request.Host}{unencodedPathAndQuery}";
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var data = _spaResponseBuilder.Make();
        var timeout = 3000;
        var requestPathBase = Request.PathBase.ToString();

        var prerenderResult = await Prerenderer.RenderToString(
            "/",
            nodeServices,
            cancellationToken,
            new JavaScriptModuleExport(
                Path.Combine(new string[] {
                    applicationBasePath,
                    "ClientApp",
                    "dist",
                    "renderer.js"
                })
            ),
            unencodedAbsoluteUrl,
            unencodedPathAndQuery,
            data,
            timeout,
            Request.PathBase.ToString()
        );

        ViewData["html"] = prerenderResult.Html;

        Response.StatusCode = prerenderResult.StatusCode != null
            ? (int)prerenderResult.StatusCode
            : (int)HttpStatusCode.OK;

        return View(prerenderResult.Globals);
    }
}
