#nullable enable

using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area admins controller
/// </summary>
[Authorize(Roles = "Admin")]
[Area(nameof(AdminArea))]
public class AdminsController : Controller
{
    private readonly IAppBLL _appBLL;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Admin area admins controller constructor
    /// </summary>
    /// <param name="userManager">Manager for the user's</param>
    /// <param name="appBLL">AppBLL</param>
    public AdminsController(UserManager<AppUser> userManager, IAppBLL appBLL)
    {
        _userManager = userManager;
        _appBLL = appBLL;
    }

    // GET: AdminArea/Admins
    /// <summary>
    /// Admin area admins controller index
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.Admins.GetAllAdminsOrderedByLastNameAsync();
        
        return View(res);
    }

    // GET: AdminArea/Admins/Details/5
    /// <summary>
    /// Admin area admins controller GET method details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
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
    /// <summary>
    /// Admin area admins controller GET method create
    /// </summary>
    /// <returns>View</returns>
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
    /// <summary>
    /// Admin area admins controller POST method create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <param name="admin">Admin</param>
    /// <returns>View</returns>
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
    /// <summary>
    /// Admin area admins controller GET method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new EditAdminViewModel();
        if (id == null) return NotFound();

        var admin = await _appBLL.Admins.FirstOrDefaultAsync(id.Value);
        if (admin == null) return NotFound();

        vm.FirstName = admin.AppUser!.FirstName;
        vm.LastName = admin.AppUser!.LastName;
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
    /// <summary>
    /// Admin area admins controller POST method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditAdminViewModel vm)
    {
        var admin = await _appBLL.Admins.FirstOrDefaultAsync(id, noIncludes: true, noTracking: true);
        if (admin != null)
        {
            admin.AppUser = null;
            
            if (id != admin.Id) return NotFound();
            
            if (ModelState.IsValid)
            {
                var appUser =
                    await _appBLL.AppUsers.GettingAppUserByAppUserIdAsync(admin.AppUserId, noIncludes: true,
                        noTracking: true);
                
                appUser.FirstName = vm.FirstName;
                appUser.LastName = vm.LastName;
                appUser.Gender = vm.Gender;
                appUser.DateOfBirth = vm.DateOfBirth;
                appUser.PhoneNumber = vm.PhoneNumber;
                
                admin.Address = vm.Address;
                admin.CityId = vm.CityId;
                admin.PersonalIdentifier = vm.PersonalIdentifier;
                admin.UpdatedBy = User.Identity!.Name!;
                admin.UpdatedAt = DateTime.Now;
                
                _appBLL.AppUsers.Update(appUser);
                _appBLL.Admins.Update(admin);
                await _appBLL.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
        }
        
        return View(vm);
    }

    // GET: AdminArea/Admins/Delete/5
    /// <summary>
    /// Admin area admins controller GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
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
    /// <summary>
    /// Admin area admins controller POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index</returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var admin = await _appBLL.Admins.FirstOrDefaultAsync(id);
        if (admin != null)
        {
            admin.AppUser = null;

            var appUser = await _userManager.FindByIdAsync(admin.AppUserId.ToString());
            await _userManager.RemoveFromRoleAsync(appUser!, "Admin");
            await _appBLL.SaveChangesAsync();

            var claims = await _userManager.GetClaimsAsync(appUser!);
            await _userManager.RemoveClaimsAsync(appUser!, claims);

           _appBLL.Admins.Remove(admin.Id, true);
            await _userManager.DeleteAsync(appUser!);
            await _appBLL.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool AdminExists(Guid id)
    {
        return _appBLL.Admins.Exists(id);
    }
}