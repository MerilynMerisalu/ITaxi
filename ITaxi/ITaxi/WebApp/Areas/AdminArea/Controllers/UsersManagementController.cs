
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize]
    public class UsersManagementController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public UsersManagementController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        // GET: UsersManagementController
        public async Task<ActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users );
        }

        // GET: UsersManagementController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        
        

        // GET: UsersManagementController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UsersManagementController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UsersManagementController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersManagementController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
