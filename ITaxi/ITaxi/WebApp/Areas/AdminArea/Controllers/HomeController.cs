using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.AdminArea.Controllers
{
    using Base.Contracts.Services;
    using Microsoft.AspNetCore.Mvc;

    public class TestController : Controller
    {
        private readonly ICurrentUserService _currentUserService;

        public TestController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public IActionResult Me()
        {
            var result = new
            {
                UserId = _currentUserService.UserId,
                UserEmail = _currentUserService.UserEmail,
                UserName = _currentUserService.UserName,
                IsAuthenticated = _currentUserService.IsAuthenicated,
                IsAdmin = _currentUserService.IsInRole("Admin"),
                IsDriver = _currentUserService.IsInRole("Driver")
            };

            return Json(result);
        }
    }
}
