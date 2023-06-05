#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area drivers controller
/// </summary>
[Area(nameof(AdminArea))]
[Authorize(Roles = "Admin")]
public class DriversController : Controller
{
    private readonly IAppBLL _appBLL;
    private readonly UserManager<App.Domain.Identity.AppUser> _userManager;
    
    /// <summary>
    /// Admin area drivers controller constructor
    /// </summary>
    /// <param name="userManager">Manager for the user's</param>
    /// <param name="appBLL">AppBLL</param>
    public DriversController(UserManager<App.Domain.Identity.AppUser> userManager, IAppBLL appBLL)
    {
        _userManager = userManager;
        _appBLL = appBLL;
    }

    // GET: AdminArea/Drivers
    /// <summary>
    /// Admin area drivers controller index
    /// </summary>
    /// <returns>iVew to the response</returns>
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync();

        return View(res);
    }

    // GET: AdminArea/Drivers/Details/5
    /// <summary>
    /// Admin area drivers controller details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteDriverViewModel();
        if (id == null) return NotFound();

        var driver = await _appBLL.Drivers.FirstOrDefaultAsync(id.Value);
        if (driver != null)
        {
            vm.Id = driver.Id;
            vm.FirstName = driver.AppUser!.FirstName;
            vm.LastName = driver.AppUser!.LastName;
            vm.LastAndFirstName = driver.AppUser!.LastAndFirstName;
            vm.Gender = driver.AppUser!.Gender;
            vm.DateOfBirth = driver.AppUser!.DateOfBirth;
            vm.DriverLicenseCategoryNames = await _appBLL.DriverAndDriverLicenseCategories
                .GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(driver!.Id);
            vm.PersonalIdentifier = driver.PersonalIdentifier;
            vm.CityName = driver.City!.CityName;
            vm.DriverLicenseNumber = driver.DriverLicenseNumber;
            vm.DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate.ToLocalTime();
            vm.Address = driver.Address;
            vm.PhoneNumber = driver.AppUser.PhoneNumber;
            vm.EmailAddress = driver.AppUser!.Email;
            vm.CreatedBy = driver.CreatedBy!;
            vm.CreatedAt = driver.CreatedAt;
            vm.UpdatedBy = driver.UpdatedBy!;
            vm.UpdatedAt = driver.UpdatedAt;
        }
        return View(vm);
    }

    // GET: AdminArea/Drivers/Create
    /// <summary>
    /// Admin area drivers controller GET method create
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Create()
    {
        var vm = new CreateDriverViewModel();
        vm.Cities = new SelectList(await _appBLL.Cities.GetAllOrderedCitiesAsync(), 
            nameof(CityDTO.Id),
            nameof(CityDTO.CityName));
        vm.DriverLicenseCategories = new SelectList(
            await _appBLL.DriverLicenseCategories.GetAllDriverLicenseCategoriesOrderedAsync(),
            nameof(DriverLicenseCategoryDTO.Id), 
            nameof(DriverLicenseCategoryDTO.DriverLicenseCategoryName));
        return View(vm);
    }

    // POST: AdminArea/Drivers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area drivers controller POST method create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <param name="driver">Driver</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDriverViewModel vm, DriverDTO driver)
    {
        if (ModelState.IsValid)
        {
            driver.Id = Guid.NewGuid();
            driver.AppUser!.FirstName = vm.FirstName;
            driver.AppUser!.LastName = vm.LastName;
            driver.AppUser!.Gender = vm.Gender;
            driver.AppUser.DateOfBirth = DateTime.Parse(vm.DateOfBirth).ToUniversalTime();
            driver.PersonalIdentifier = vm.PersonalIdentifier;
            driver.CityId = vm.CityId;
            driver.Address = vm.Address;
            driver.DriverLicenseNumber = vm.DriverLicenseNumber;
            driver.DriverLicenseExpiryDate = DateTime.Parse(vm.DriverLicenseExpiryDate).ToUniversalTime();
            _appBLL.Drivers.Add(driver);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.Cities = new SelectList(await _appBLL.Cities.GetAllOrderedCitiesWithoutCountyAsync(), 
            nameof(CityDTO.Id),
            nameof(CityDTO.CityName), driver.CityId);
        return View(vm);
    }

    // GET: AdminArea/Drivers/Edit/5
    /// <summary>
    /// Admin area drivers controller GET method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var driver = await _appBLL.Drivers
            .FirstOrDefaultAsync(id.Value);

        var vm = new EditDriverViewModel();
        vm.DriverLicenseCategories = new SelectList(
            await _appBLL
                .DriverLicenseCategories.GetAllDriverLicenseCategoriesOrderedAsync(),
            nameof(DriverLicenseCategoryDTO.Id),
            nameof(DriverLicenseCategoryDTO.DriverLicenseCategoryName));
        vm.Cities = new SelectList(await _appBLL.Cities.GetAllOrderedCitiesAsync(),
            nameof(CityDTO.Id), nameof(CityDTO.CityName));
        if (driver != null)
        {
            vm.FirstName = driver.AppUser!.FirstName;
            vm.LastName = driver.AppUser!.LastName;
            vm.Gender = driver.AppUser!.Gender;
            vm.Address = driver.Address;
            vm.CityId = driver.CityId;
            vm.PersonalIdentifier = driver.PersonalIdentifier;
            vm.DateOfBirth = driver.AppUser!.DateOfBirth;
            vm.DriverLicenseNumber = driver.DriverLicenseNumber;
            vm.DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate;
            vm.PhoneNumber = driver!.AppUser!.PhoneNumber;
            vm.Email = driver.AppUser!.Email;
        }
        return View(vm);
    }

    // POST: AdminArea/Drivers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area drivers controller POST method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditDriverViewModel vm)
    {
        var driver = await _appBLL.Drivers.FirstOrDefaultAsync(id, noIncludes:true, noTracking:true);
        var appuser = await _appBLL.AppUsers.GettingAppUserByAppUserIdAsync(driver!.AppUserId, noIncludes: true);

        if (driver != null && id != driver.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (driver != null)
                {

                    driver.PersonalIdentifier = vm.PersonalIdentifier;
                    if (vm.DriverAndDriverLicenseCategories != null)
                    {
                        await _appBLL.DriverAndDriverLicenseCategories
                            .RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(driver.Id);

                        await _appBLL.SaveChangesAsync();
                        
                        foreach (var selectedDriverLicenseCategory in vm.DriverAndDriverLicenseCategories)
                        {
                            var driverAndDriverLicenseCategory = new DriverAndDriverLicenseCategoryDTO()
                            {
                                DriverId = driver.Id,
                                DriverLicenseCategoryId = selectedDriverLicenseCategory
                            };
                            _appBLL.DriverAndDriverLicenseCategories.Add(driverAndDriverLicenseCategory);
                        }
                    }
                    
                    appuser.FirstName = vm.FirstName;
                    appuser.LastName = vm.LastName;
                    appuser.Gender = vm.Gender;
                    appuser.DateOfBirth = DateTime.Parse(vm.DateOfBirth.ToString("d")).ToUniversalTime();
                    appuser.PhoneNumber = vm.PhoneNumber;
                    appuser.Email = vm.Email;
                    appuser.IsActive = vm.IsActive;
                    _appBLL.AppUsers.Update(appuser);
                    
                    driver.DriverLicenseNumber = vm.DriverLicenseNumber;
                    driver.DriverLicenseExpiryDate = DateTime.Parse(vm.DriverLicenseExpiryDate.ToString("d")).ToUniversalTime();
                    driver.CityId = vm.CityId;
                    driver.Address = vm.Address;
                    driver.UpdatedBy = User.Identity!.Name!;
                    driver.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _appBLL.Drivers.Update(driver);
                }

                await _appBLL.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(driver!.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/Drivers/Delete/5
    /// <summary>
    /// Admin area drivers controller GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteDriverViewModel();
        if (id == null) return NotFound();

        var driver = await _appBLL.Drivers.FirstOrDefaultAsync(id.Value);
        if (driver == null) return NotFound();

        vm.Id = driver.Id;
        vm.FirstName = driver.AppUser!.FirstName;
        vm.LastName = driver.AppUser!.LastName;
        vm.LastAndFirstName = driver.AppUser!.LastAndFirstName;
        vm.Gender = driver.AppUser!.Gender;
        vm.DateOfBirth = driver.AppUser!.DateOfBirth;
        vm.DriverLicenseCategoryNames = await _appBLL.DriverAndDriverLicenseCategories
            .GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(driver!.Id);
        vm.PersonalIdentifier = driver.PersonalIdentifier;
        vm.CityName = driver.City!.CityName;
        vm.DriverLicenseNumber = driver.DriverLicenseNumber;
        vm.DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate.ToLocalTime();
        vm.Address = driver.Address;
        vm.PhoneNumber = driver.AppUser.PhoneNumber;
        vm.EmailAddress = driver.AppUser!.Email;
        vm.CreatedBy = driver.CreatedBy!;
        vm.CreatedAt = driver.CreatedAt;
        vm.UpdatedBy = driver.UpdatedBy!;
        vm.UpdatedAt = driver.UpdatedAt;
        return View(vm);
    }

    // POST: AdminArea/Drivers/Delete/5
    /// <summary>
    /// Admin area drivers controller POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var noTracking = true;
        var driver = await _appBLL.Drivers.FirstOrDefaultAsync(id, noTracking, noIncludes:true);
        if (await _appBLL.Drivers.HasAnySchedulesAsync(id) || await _appBLL.Drivers.HasAnyBookingsAsync(id))
            return Content("Entity cannot be deleted because it has dependent entities!");

        await _appBLL.DriverAndDriverLicenseCategories.
            RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(id, noTracking);
        
        if (driver != null)
        {
            driver.AppUser = null;
            var appUser = await _userManager.FindByIdAsync(driver!.AppUserId.ToString());
            await _userManager.RemoveFromRoleAsync(appUser!, "Driver");
            await _appBLL.Drivers.RemoveAsync(driver.Id);
            await _appBLL.SaveChangesAsync();
            
            var claims = await _userManager.GetClaimsAsync(appUser!);
            await _userManager.RemoveClaimsAsync(appUser!, claims);
            await _userManager.DeleteAsync(appUser!);
            await _appBLL.SaveChangesAsync();
        }

        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DriverExists(Guid id)
    {
        return _appBLL.Drivers.Exists(id);
    }
}