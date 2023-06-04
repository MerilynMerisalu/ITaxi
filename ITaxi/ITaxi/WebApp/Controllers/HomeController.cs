using System.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

/// <summary>
/// Home controller
/// </summary>
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    /// <summary>
    /// Constructor for home controller
    /// </summary>
    /// <param name="logger"></param>
    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Home controller index
    /// </summary>
    /// <returns>Empty view</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Home controller privacy
    /// </summary>
    /// <returns>Empty view</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Home controller test action
    /// </summary>
    /// <param name="a">A</param>
    /// <param name="b">B</param>
    /// <returns>New error view model</returns>
    public string TestAction(int a, int b)
    {
        return (a + b).ToString();
    }
    /// <summary>
    /// Home controller error view
    /// </summary>
    /// <returns>New error view model</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }

    /// <summary>
    /// Home controller set language method
    /// </summary>
    /// <param name="culture">Culture</param>
    /// <param name="returnUrl">Return language url</param>
    /// <returns>Url for the language</returns>
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddYears(1)
            });
        return LocalRedirect(returnUrl);
    }
}