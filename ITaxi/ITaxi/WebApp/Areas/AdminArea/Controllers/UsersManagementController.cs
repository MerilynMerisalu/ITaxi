
using App.Contracts.BLL;
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
        private readonly IAppBLL _appBLL;
        private readonly IUserManagementService _userManagementService;

        public UsersManagementController(IUserManagementService userManagementService, IAppBLL appBLL)
        {
            _userManagementService = userManagementService;
            _appBLL = appBLL;
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
            var admin = await _appBLL.Admins.GetAdminByAppUserIdAsync(user.Id);
            var driver = await _appBLL.Drivers.GettingDriverByAppUserIdAsync(user.Id);
            var customer = await _appBLL.Customers.GettingCustomerByAppuserIdAsync(user.Id);
                
            var vm = new UserManagementViewModel();
            vm.Id = user.Id;
            vm.FirstName = user.FirstName;
            vm.LastName = user.LastName;
            vm.Role = user.Role;
            vm.Gender = user.Gender;
            vm.DateOfBirth = user.DateOfBirth;
            vm.EmailAddress = user.EmailAddress;
            vm.PhoneNumber = user.PhoneNumber;
            if (admin != null)
            {
                vm.AdminId = admin.Id;
                vm.PersonalIdentifier = admin.PersonalIdentifier;
                vm.Country = admin.City.County.Country.CountryName;
                vm.County = admin.City.County.CountyName;
                vm.City = admin.City.CityName;
                vm.AddressOfResidence = admin.Address;
            }
            if (driver != null)
            {
                vm.DriverId = driver.Id;
                vm.PersonalIdentifier = driver.PersonalIdentifier;
                vm.Country = driver.City.County.Country.CountryName;
                vm.County = driver.City.County.CountyName;
                vm.City = driver.City.CityName;
                vm.AddressOfResidence = driver.Address;
                
            }



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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShowHide(Guid? adminId = null, Guid? driverId = null, Guid? customerId = null)
        {
            if (adminId != null)
            {
                var result = await _appBLL.Admins.ToggleIsIgnoredAsync(id: adminId.Value, showIgnored: true);
                return RedirectToAction(nameof(Index));
            }
            else if (driverId != null)
            {
                var result = await _appBLL.Drivers.ToggleIsIgnoredAsync(id: driverId.Value, showIgnored: true);
                return RedirectToAction(nameof(Index));
            }

            else
            {
                var result = await _appBLL.Customers.ToggleIsIgnoredAsync(id: customerId.Value, showIgnored: true);
                return RedirectToAction(nameof(Index));
            }


        }
    }
}
