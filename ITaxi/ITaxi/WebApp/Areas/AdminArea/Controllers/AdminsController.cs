#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area(nameof(AdminArea))]
    public class AdminsController : Controller
    {
        private readonly IAppUnitOfWork _uow;
        
        public AdminsController(IAppUnitOfWork uow)
        {
            _uow = uow;
            
        }

        // GET: AdminArea/Admins
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Admins.GetAllAdminsOrderedByLastNameAsync());
        }

        // GET: AdminArea/Admins/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteAdminViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _uow.Admins.FirstOrDefaultAsync(id.Value);
            if (admin == null)
            {
                return NotFound();
            }


            vm.FirstName = admin.AppUser!.FirstName;
            vm.LastName = admin.AppUser!.LastName;
            vm.LastAndFirstName = admin.AppUser!.LastAndFirstName;
            vm.Gender = admin.AppUser!.Gender;
            vm.DateOfBirth = admin.AppUser!.DateOfBirth;
            vm.Address = admin.Address;
            vm.City = admin.City;
            vm.PhoneNumber = admin.AppUser!.PhoneNumber;
            vm.IsActive = admin.AppUser!.IsActive;
            if (admin.PersonalIdentifier != null) vm.PersonalIdentifier = admin.PersonalIdentifier;
            vm.Id = admin.Id;

            return View(vm);
        }

        // GET: AdminArea/Admins/Create
        /*
        public IActionResult Create()
        {
            
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName");
            return View();
        }

        // POST: AdminArea/Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppUserId,PersonalIdentifier,CityId,Address,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                admin.Id = Guid.NewGuid();
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Email", admin.AppUserId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", admin.CityId);
            return View(admin);
        }
        */

        // GET: AdminArea/Admins/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new EditAdminViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _uow.Admins.FirstOrDefaultAsync(id.Value);
            if (admin == null)
            {
                return NotFound();
            }

            vm.FirstName = admin.AppUser!.FirstName;
            vm.LastName = admin.AppUser!.LastName;
            vm.DateOfBirth = admin.AppUser!.DateOfBirth;
            vm.PersonalIdentifier = admin.PersonalIdentifier;
            vm.Gender = admin.AppUser!.Gender;
            vm.CityId = admin.CityId;
            vm.Address = admin.Address;
            vm.Cities = new SelectList(await _uow.Cities.GetAllOrderedCitiesAsync(),
            nameof(City.Id), nameof(City.CityName));
            return View(vm);
        }

        // POST: AdminArea/Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditAdminViewModel vm)
        {
            var admin = await _uow.Admins.FirstOrDefaultAsync(id);
            if (admin != null && id != admin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (admin != null)
                    {
                        
                        admin.Address = vm.Address;
                        admin.CityId = vm.CityId;
                        admin.PersonalIdentifier = vm.PersonalIdentifier;
                        admin.UpdatedAt = DateTime.UtcNow;
                        _uow.Admins.Update(admin);
                    }

                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (admin != null && !AdminExists(admin.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(vm);
        }

        // GET: AdminArea/Admins/Delete/5
        public async Task<IActionResult> Delete(Guid? id, DetailsDeleteAdminViewModel vm)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _uow.Admins.FirstOrDefaultAsync(id.Value);
            if (admin == null)
            {
                return NotFound();
            }

            vm.FirstName = admin.AppUser!.FirstName;
            vm.LastName = admin.AppUser!.LastName;
            vm.LastAndFirstName = admin.AppUser!.LastAndFirstName;
            vm.Gender = admin.AppUser!.Gender;
            vm.DateOfBirth = admin.AppUser!.DateOfBirth;
            vm.Address = admin.Address;
            vm.City = admin.City;
            vm.PhoneNumber = admin.AppUser!.PhoneNumber;
            vm.IsActive = admin.AppUser!.IsActive;
            if (admin.PersonalIdentifier != null) vm.PersonalIdentifier = admin.PersonalIdentifier;
            return View(vm);
        }

        // POST: AdminArea/Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            #warning Try to figure out how to combine UOW pattern with users 
            var admin = await _uow.Admins.FirstOrDefaultAsync(id);

            if (admin != null)
            {
               
                _uow.Admins.Remove(admin);
                await _uow.SaveChangesAsync();
                
            }

            
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(Guid id)
        {
            return _uow.Admins.Exists(id);
        }
    }
}
