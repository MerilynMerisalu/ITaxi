
using App.Contracts.BLL.Services;
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
        private readonly IUserManagementService _userManagementService;

        public UsersManagementController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        // GET: UsersManagementController
        public async Task<ActionResult> Index()
        {
            var users = await _userManagementService.GetUsersAsync();
            return View(users);
           
        }

        // GET: UsersManagementController/Details/5
        public async Task<ActionResult> Details(Guid? id)
            {
            
            var user = await _userManagementService.GetUserByIdAsync(id.Value);
            if (user == null)
            {
                return NotFound();
            }
            var vm = new UserManagementViewModel();
            vm.Id = user.Id;
            vm.FirstName = user.FirstName;
            vm.LastName = user.LastName;
            vm.Role = user.Role;
            vm.EmailAddress = user.EmailAddress;
            vm.PhoneNumber = user.PhoneNumber;

            
            return View(vm);
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
