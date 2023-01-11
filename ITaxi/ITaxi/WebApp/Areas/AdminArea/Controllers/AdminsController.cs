#nullable enable
using System.Security.Claims;
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Contracts.DAL;


using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;


namespace WebApp.Areas.AdminArea.Controllers;

[Authorize(Roles = "Admin")]
[Area(nameof(AdminArea))]
public class AdminsController : Controller
{
    private readonly IAppBLL _appBLL;
    private readonly UserManager<AppUser> _userManager;

    public AdminsController(UserManager<AppUser> userManager, IAppBLL appBLL)
    {
        _userManager = userManager;
        _appBLL = appBLL;
    }

    // GET: AdminArea/Admins
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.Admins.GetAllAdminsOrderedByLastNameAsync();


        return View(res);
    }

    // GET: AdminArea/Admins/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteAdminViewModel();
        if (id == null) return NotFound();

        var admin = await _appBLL.Admins.FirstOrDefaultAsync(id.Value);
        if (admin == null) return NotFound();


        vm.FirstName = admin.AppUser!.FirstName;
        vm.LastName = admin.AppUser!.LastName;
        vm.LastAndFirstName = admin.AppUser!.LastAndFirstName;
        vm.Gender = admin.AppUser!.Gender;
        vm.DateOfBirth = admin.AppUser!.DateOfBirth;
        vm.Address = admin.Address;
        vm.City = admin.City!.CityName;
        vm.PhoneNumber = admin.AppUser!.PhoneNumber;
        vm.Email = admin.AppUser!.Email;
        vm.IsActive = admin.AppUser!.IsActive;
        if (admin.PersonalIdentifier != null) vm.PersonalIdentifier = admin.PersonalIdentifier;

        vm.Id = admin.Id;
        vm.CreatedBy = admin.CreatedBy!;
        vm.CreatedAt = admin.CreatedAt;
        vm.UpdatedBy = admin.UpdatedBy!;
        vm.UpdatedAt = admin.UpdatedAt;

        return View(vm);
    }

    // GET: AdminArea/Admins/Create

    public async Task<IActionResult> Create()
    {
        var vm = new CreateAdminViewModel();
        vm.Cities = new SelectList(await _appBLL.Cities.GetAllOrderedCitiesAsync(),
            nameof(CityDTO.Id), nameof(App.BLL.DTO.AdminArea.CityDTO.CityName));

        return View(vm);
    }

    // POST: AdminArea/Admins/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateAdminViewModel vm, AdminDTO admin)
    {
        if (ModelState.IsValid)
        {
            admin.Id = Guid.NewGuid();
            _appBLL.Admins.Add(admin);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.Cities = new SelectList(await _appBLL.Cities.GetAllOrderedCitiesAsync(), nameof(CityDTO.Id),
            nameof(CityDTO.CityName), nameof(vm.CityId));
        return View(vm);
    }


    // GET: AdminArea/Admins/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new EditAdminViewModel();
        if (id == null) return NotFound();

        var admin = await _appBLL.Admins.FirstOrDefaultAsync(id.Value);
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
        vm.Cities = new SelectList(await _appBLL.Cities.GetAllOrderedCitiesAsync(),
            nameof(CityDTO.Id), nameof(CityDTO.CityName));
        return View(vm);
    }

    // POST: AdminArea/Admins/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditAdminViewModel vm)
    {
        var admin = await _appBLL.Admins.FirstOrDefaultAsync(id);
        
        if (admin != null && id != admin.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (admin != null)
                {
                    /*
                    admin.AppUser!.FirstName = vm.FirstName;
                    admin.AppUser!.LastName = vm.LastName;
                    admin.AppUser!.Gender = vm.Gender;
                    admin.AppUser!.DateOfBirth = DateTime.Parse(vm.DateOfBirth.ToString("d"))
                        .ToUniversalTime();
                    admin.AppUser!.PhoneNumber = vm.PhoneNumber;
                    admin.AppUser!.Email = vm.Email;
                    //_a
                    */
                    admin.Address = vm.Address;
                    admin.CityId = vm.CityId;
                    admin.PersonalIdentifier = vm.PersonalIdentifier;
                    //admin.AppUser!.IsActive = vm.IsActive;
                    admin.UpdatedBy = User.Identity!.Name!;
                    admin.UpdatedAt = DateTime.Now;
                    _appBLL.Admins.Update(admin);
                }

                await _appBLL.SaveChangesAsync();
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

        var admin = await _appBLL.Admins.FirstOrDefaultAsync(id.Value);
        if (admin == null) return NotFound();

        vm.FirstName = admin.AppUser!.FirstName;
        vm.LastName = admin.AppUser!.LastName;
        vm.LastAndFirstName = admin.AppUser!.LastAndFirstName;
        vm.Gender = admin.AppUser!.Gender;
        vm.DateOfBirth = admin.AppUser!.DateOfBirth;
        vm.Address = admin.Address;
        vm.City = admin.City!.CityName;
        vm.PhoneNumber = admin.AppUser!.PhoneNumber;
        vm.Email = admin.AppUser.Email;
        vm.IsActive = admin.AppUser!.IsActive;
        if (admin.PersonalIdentifier != null) vm.PersonalIdentifier = admin.PersonalIdentifier;

        vm.CreatedBy = admin.CreatedBy!;
        vm.CreatedAt = admin.CreatedAt;
        vm.UpdatedBy = admin.UpdatedBy!;
        vm.UpdatedAt = admin.UpdatedAt;

        return View(vm);
    }

    // POST: AdminArea/Admins/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {

        var admin = await _appBLL.Admins.FirstOrDefaultAsync(id);
        if (admin != null)
        {
            var appUser = await _userManager.FindByEmailAsync(admin.AppUser!.Email);
            await _userManager.RemoveFromRoleAsync(appUser, "Admin");
            _appBLL.Admins.Remove(admin);
            await _appBLL.SaveChangesAsync();
            #warning temporarily solution
            var claims = await _userManager.GetClaimsAsync(appUser);
            await _userManager.RemoveClaimsAsync(appUser, claims);
            await _userManager.DeleteAsync(appUser);
           await _appBLL.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool AdminExists(Guid id)
    {
        return _appBLL.Admins.Exists(id);
    }
}