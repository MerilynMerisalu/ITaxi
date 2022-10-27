#nullable enable
using App.Contracts.DAL;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

[Authorize(Roles = nameof(Admin))]
[Area(nameof(AdminArea))]
public class AdminsController : Controller
{
    private readonly IAppUnitOfWork _uow;
    private readonly UserManager<AppUser> _userManager;

    public AdminsController(IAppUnitOfWork uow, UserManager<AppUser> userManager)
    {
        _uow = uow;
        _userManager = userManager;
    }

    // GET: AdminArea/Admins
    public async Task<IActionResult> Index()
    {
        var res = await _uow.Admins.GetAllAdminsOrderedByLastNameAsync();
#warning Should this be a repo method
        foreach (var admin in res)
        {
            admin.AppUser!.DateOfBirth = admin.AppUser.DateOfBirth.ToLocalTime();
            admin.CreatedAt = admin.CreatedAt.ToLocalTime();
            admin.UpdatedAt = admin.UpdatedAt.ToLocalTime();
        }

        return View(res);
    }

    // GET: AdminArea/Admins/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteAdminViewModel();
        if (id == null) return NotFound();

        var admin = await _uow.Admins.FirstOrDefaultAsync(id.Value);
        if (admin == null) return NotFound();


        vm.FirstName = admin.AppUser!.FirstName;
        vm.LastName = admin.AppUser!.LastName;
        vm.LastAndFirstName = admin.AppUser!.LastAndFirstName;
        vm.Gender = admin.AppUser!.Gender;
        vm.DateOfBirth = admin.AppUser!.DateOfBirth;
        vm.Address = admin.Address;
        vm.City = admin.City;
        vm.PhoneNumber = admin.AppUser!.PhoneNumber;
        vm.Email = admin.AppUser!.Email;
        vm.IsActive = admin.AppUser!.IsActive;
        if (admin.PersonalIdentifier != null) vm.PersonalIdentifier = admin.PersonalIdentifier;

        vm.Id = admin.Id;
        vm.CreatedBy = admin.CreatedBy!;
        vm.CreatedAt = admin.CreatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = admin.UpdatedBy!;
        vm.UpdatedAt = admin.UpdatedAt.ToLocalTime().ToString("G");

        return View(vm);
    }

    // GET: AdminArea/Admins/Create

    public async Task<IActionResult> Create()
    {
        var vm = new CreateAdminViewModel();
        vm.Cities = new SelectList(await _uow.Cities.GetAllOrderedCitiesAsync(),
            nameof(City.Id), nameof(City.CityName));

        return View(vm);
    }

    // POST: AdminArea/Admins/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateAdminViewModel vm, Admin admin)
    {
        if (ModelState.IsValid)
        {
            admin.Id = Guid.NewGuid();
            _uow.Admins.Add(admin);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.Cities = new SelectList(await _uow.Cities.GetAllOrderedCitiesAsync(), nameof(City.Id),
            nameof(City.CityName), nameof(vm.CityId));
        return View(vm);
    }


    // GET: AdminArea/Admins/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new EditAdminViewModel();
        if (id == null) return NotFound();

        var admin = await _uow.Admins.FirstOrDefaultAsync(id.Value);
        if (admin == null) return NotFound();

        vm.FirstName = admin.AppUser!.FirstName;
        vm.LastName = admin.AppUser!.LastName;
#warning ask if there is a better way
        vm.DateOfBirth = admin.AppUser.DateOfBirth;
        vm.PersonalIdentifier = admin.PersonalIdentifier;
        vm.Gender = admin.AppUser!.Gender;
        vm.CityId = admin.CityId;
        vm.Address = admin.Address;
        vm.PhoneNumber = admin.AppUser!.PhoneNumber;
        vm.Email = admin.AppUser.Email;
        vm.IsActive = admin.AppUser!.IsActive;
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
        
        if (admin != null && id != admin.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (admin != null)
                {
                    admin.AppUser!.FirstName = vm.FirstName;
                    admin.AppUser!.LastName = vm.LastName;
                    admin.AppUser!.Gender = vm.Gender;
                    admin.AppUser!.DateOfBirth = DateTime.Parse(vm.DateOfBirth.ToString("d"))
                        .ToUniversalTime();
                    admin.AppUser!.PhoneNumber = vm.PhoneNumber;
                    admin.AppUser!.Email = vm.Email;
                    admin.Address = vm.Address;
                    admin.CityId = vm.CityId;
                    admin.PersonalIdentifier = vm.PersonalIdentifier;
                    admin.AppUser!.IsActive = vm.IsActive;
                    admin.UpdatedBy = User.Identity!.Name!;
                    admin.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _uow.Admins.Update(admin);
                }

                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (admin == null)
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/Admins/Delete/5
    public async Task<IActionResult> Delete(Guid? id, DetailsDeleteAdminViewModel vm)
    {
        if (id == null) return NotFound();

        var admin = await _uow.Admins.FirstOrDefaultAsync(id.Value);
        if (admin == null) return NotFound();

        vm.FirstName = admin.AppUser!.FirstName;
        vm.LastName = admin.AppUser!.LastName;
        vm.LastAndFirstName = admin.AppUser!.LastAndFirstName;
        vm.Gender = admin.AppUser!.Gender;
        vm.DateOfBirth = admin.AppUser!.DateOfBirth;
        vm.Address = admin.Address;
        vm.City = admin.City;
        vm.PhoneNumber = admin.AppUser!.PhoneNumber;
        vm.Email = admin.AppUser.Email;
        vm.IsActive = admin.AppUser!.IsActive;
        if (admin.PersonalIdentifier != null) vm.PersonalIdentifier = admin.PersonalIdentifier;

        vm.CreatedBy = admin.CreatedBy!;
        vm.CreatedAt = admin.CreatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = admin.UpdatedBy!;
        vm.UpdatedAt = admin.UpdatedAt.ToLocalTime().ToString("G");

        return View(vm);
    }

    // POST: AdminArea/Admins/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
#warning Try to figure out how to combine UOW pattern with users
        var admin = await _uow.Admins.FirstOrDefaultAsync(id);
        if (admin != null)
        {
            var appUser = await _userManager.FindByIdAsync(admin!.AppUserId.ToString());

#warning Ask how to delete an user when using uow
            await _userManager.RemoveFromRoleAsync(appUser, nameof(admin));
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