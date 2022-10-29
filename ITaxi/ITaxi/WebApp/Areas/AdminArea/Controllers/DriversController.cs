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

[Area(nameof(AdminArea))]
[Authorize(Roles = nameof(Admin))]
public class DriversController : Controller
{
    private readonly IAppUnitOfWork _uow;
    private readonly UserManager<AppUser> _userManager;


    public DriversController(IAppUnitOfWork uow, UserManager<AppUser> userManager)
    {
        _uow = uow;
        _userManager = userManager;
    }

    // GET: AdminArea/Drivers
    public async Task<IActionResult> Index()
    {
        var res = await _uow.Drivers.GetAllDriversOrderedByLastNameAsync();
#warning Should this be a repo method
        foreach (var driver in res)
        {
            driver.AppUser!.DateOfBirth = driver.AppUser.DateOfBirth.ToLocalTime();
            driver.DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate.ToLocalTime();
            driver.CreatedAt = driver.CreatedAt.ToLocalTime();
            driver.UpdatedAt = driver.UpdatedAt.ToLocalTime();
        }

        return View(res);
    }

    // GET: AdminArea/Drivers/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteDriverViewModel();
        if (id == null) return NotFound();

        var driver = await _uow.Drivers.FirstOrDefaultAsync(id.Value);
        if (driver != null)
        {
            vm.Id = driver.Id;
            vm.FirstName = driver.AppUser!.FirstName;
            vm.LastName = driver.AppUser!.LastName;
            vm.LastAndFirstName = driver.AppUser!.LastAndFirstName;
            vm.Gender = driver.AppUser!.Gender;
            vm.DateOfBirth = driver.AppUser!.DateOfBirth;
            vm.DriverLicenseCategoryNames = await _uow.DriverAndDriverLicenseCategories
                .GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(driver!.Id);
            vm.PersonalIdentifier = driver.PersonalIdentifier;
            vm.CityName = driver.City!.CityName;
            vm.DriverLicenseNumber = driver.DriverLicenseNumber;
            vm.DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate.ToLocalTime();
            vm.Address = driver.Address;
            vm.PhoneNumber = driver.AppUser.PhoneNumber;
            vm.EmailAddress = driver.AppUser!.Email;
            vm.CreatedBy = driver.CreatedBy!;
            vm.CreatedAt = driver.CreatedAt.ToLocalTime().ToString("G");
            vm.UpdatedBy = driver.UpdatedBy!;
            vm.UpdatedAt = driver.UpdatedAt.ToLocalTime().ToString("G");
        }

        return View(vm);
    }

    // GET: AdminArea/Drivers/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateDriverViewModel();
        vm.Cities = new SelectList(await _uow.Cities.GetAllOrderedCitiesAsync(), nameof(City.Id),
            nameof(City.CityName));
        vm.DriverLicenseCategories = new SelectList(
            await _uow.DriverLicenseCategories.GetAllDriverLicenseCategoriesOrderedAsync(),
            nameof(DriverLicenseCategory.Id), nameof(DriverLicenseCategory.DriverLicenseCategoryName));
        return View(vm);
    }

    // POST: AdminArea/Drivers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateDriverViewModel vm, Driver driver)
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
            _uow.Drivers.Add(driver);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.Cities = new SelectList(await _uow.Cities.GetAllOrderedCitiesWithoutCountyAsync(), nameof(City.Id),
            nameof(City.CityName), driver.CityId);
        return View(vm);
    }

    // GET: AdminArea/Drivers/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var driver = await _uow.Drivers
            .FirstOrDefaultAsync(id.Value);

        var vm = new EditDriverViewModel();
        vm.DriverLicenseCategories = new SelectList(
            await _uow
                .DriverLicenseCategories.GetAllDriverLicenseCategoriesOrderedAsync(),
            nameof(DriverLicenseCategory.Id),
            nameof(DriverLicenseCategory.DriverLicenseCategoryName));
        vm.Cities = new SelectList(await _uow.Cities.GetAllOrderedCitiesAsync(),
            nameof(City.Id), nameof(City.CityName));
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
#warning Ask if this should be a repository method
            vm.DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate;
            vm.PhoneNumber = driver!.AppUser!.PhoneNumber;
            vm.Email = driver.AppUser!.Email;
        }

        return View(vm);
    }

    // POST: AdminArea/Drivers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditDriverViewModel vm)
    {
        var driver = await _uow.Drivers.FirstOrDefaultAsync(id);

        if (driver != null && id != driver.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (driver != null)
                {
                    driver.Id = id;
                    driver.PersonalIdentifier = vm.PersonalIdentifier;
                    if (vm.DriverAndDriverLicenseCategories != null)
                    {
                        await _uow.DriverAndDriverLicenseCategories
                            .RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(driver.Id);

                        foreach (var selectedDriverLicenseCategory in vm.DriverAndDriverLicenseCategories)
                        {
                            var driverAndDriverLicenseCategory = new DriverAndDriverLicenseCategory
                            {
                                DriverId = driver.Id,
                                DriverLicenseCategoryId = selectedDriverLicenseCategory
                            };
                            _uow.DriverAndDriverLicenseCategories.Add(driverAndDriverLicenseCategory);
                        }
                    }

                    driver.AppUser!.FirstName = vm.FirstName;
                    driver.AppUser!.LastName = vm.LastName;
                    driver.AppUser!.Gender = vm.Gender;
                    driver.AppUser!.DateOfBirth = DateTime.Parse(vm.DateOfBirth.ToString("d")).ToUniversalTime();
                    driver.AppUser.PhoneNumber = vm.PhoneNumber;
                    driver.AppUser.Email = vm.Email;
                    driver.AppUser.IsActive = vm.IsActive;
                    driver.DriverLicenseNumber = vm.DriverLicenseNumber;
                    driver.DriverLicenseExpiryDate = DateTime.Parse(vm.DriverLicenseExpiryDate.ToString("d")).ToUniversalTime();
                    driver.CityId = vm.CityId;
                    driver.Address = vm.Address;
                    driver.UpdatedBy = User.Identity!.Name!;
                    driver.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _uow.Drivers.Update(driver);
                }

                await _uow.SaveChangesAsync();
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
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteDriverViewModel();
        if (id == null) return NotFound();

        var driver = await _uow.Drivers.FirstOrDefaultAsync(id.Value);
        if (driver == null) return NotFound();

        vm.Id = driver.Id;
        vm.FirstName = driver.AppUser!.FirstName;
        vm.LastName = driver.AppUser!.LastName;
        vm.LastAndFirstName = driver.AppUser!.LastAndFirstName;
        vm.Gender = driver.AppUser!.Gender;
        vm.DateOfBirth = driver.AppUser!.DateOfBirth;
        vm.DriverLicenseCategoryNames = await _uow.DriverAndDriverLicenseCategories
            .GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(driver!.Id);
        vm.PersonalIdentifier = driver.PersonalIdentifier;
        vm.CityName = driver.City!.CityName;
        vm.DriverLicenseNumber = driver.DriverLicenseNumber;
        vm.DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate.ToLocalTime();
        vm.Address = driver.Address;
        vm.PhoneNumber = driver.AppUser.PhoneNumber;
        vm.EmailAddress = driver.AppUser!.Email;
        vm.CreatedBy = driver.CreatedBy!;
        vm.CreatedAt = driver.CreatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = driver.UpdatedBy!;
        vm.UpdatedAt = driver.UpdatedAt.ToLocalTime().ToString("G");
        return View(vm);
    }

    // POST: AdminArea/Drivers/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var driver = await _uow.Drivers.SingleOrDefaultAsync(d => d != null && d.Id.Equals(id));
        if (await _uow.Schedules.AnyAsync(d => driver != null && d != null && d.DriverId.Equals(driver.Id))
            || await _uow.Bookings.AnyAsync(d => driver != null && d != null && d.DriverId.Equals(driver.Id)))
            return Content("Entity cannot be deleted because it has dependent entities!");

        await _uow.DriverAndDriverLicenseCategories.RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(id);

        if (driver != null)
        {
            var appUser = await _userManager.FindByIdAsync(driver!.AppUserId.ToString());
            await _userManager.RemoveFromRoleAsync(appUser, nameof(Driver));
            _uow.Drivers.Remove(driver);
            await _uow.SaveChangesAsync();
    #warning  temporarily solution
            var claims = await _userManager.GetClaimsAsync(appUser);
            await _userManager.RemoveClaimsAsync(appUser, claims);
            await _userManager.DeleteAsync(appUser);
            await _uow.SaveChangesAsync();
        }

        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DriverExists(Guid id)
    {
        return _uow.Drivers.Exists(id);
    }
}