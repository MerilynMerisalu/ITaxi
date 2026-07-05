using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Base.Controllers;

namespace WebApp.Controllers
{
    public class ErrorController : BaseErrorController
    {
        // GET: ErrorController
        [Route("Error/{statusCode:int}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            return GetErrorView(statusCode);
        }

    }
}
