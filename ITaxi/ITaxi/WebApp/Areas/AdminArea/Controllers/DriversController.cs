#nullable enable
using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area(nameof(AdminArea))]
    public class DriversController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public DriversController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: AdminArea/Drivers
        public async Task<IActionResult> Index()
        {
            
            return View(await _uow.Drivers.GetAllDriversOrderedByLastNameAsync());
        }

        // GET: AdminArea/Drivers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteDriverViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _uow.Drivers.FirstOrDefaultAsync(id.Value);
            vm.DriverLicenseCategoryNames = await _uow.DriverAndDriverLicenseCategories
                .GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(driver!.Id);
            vm.Id = driver.Id;
            vm.PersonalIdentifier = driver.PersonalIdentifier;
            vm.CityName = driver.City!.CityName;
            vm.DriverLicenseNumber = driver.DriverLicenseNumber;
            vm.DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate;
            vm.Address = driver.Address;

            return View(vm);
        }

        // GET: AdminArea/Drivers/Create
        /*public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_uow.Users, "Id", "Email");
            ViewData["CityId"] = new SelectList(_uow.Cities, "Id", "CityName");
            return View();
        }

        // POST: AdminArea/Drivers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppUserId,PersonalIdentifier,DriverLicenseNumber,DriverLicenseExpiryDate,CityId,Address,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                driver.Id = Guid.NewGuid();
                _uow.Add(driver);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_uow.Users, "Id", "Email", driver.AppUserId);
            ViewData["CityId"] = new SelectList(_uow.Cities, "Id", "CityName", driver.CityId);
            return View(driver);
        }*/

        // GET: AdminArea/Drivers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _uow.Drivers
                .FirstOrDefaultAsync(id.Value);

            var vm = new EditDriverViewModel();
            vm.DriverLicenseCategories= new SelectList(
                await _uow
                    .DriverLicenseCategories.GetAllDriverLicenseCategoriesOrderedAsync(),
                nameof(DriverLicenseCategory.Id), 
                nameof(DriverLicenseCategory.DriverLicenseCategoryName));
            vm.Cities = new SelectList(await _uow.Cities.GetAllOrderedCitiesAsync(),
                nameof(City.Id), nameof(City.CityName));
            if (driver != null)
            {
                vm.Address = driver.Address;
                vm.CityId = driver.CityId;
                vm.PersonalIdentifier = driver.PersonalIdentifier;
                vm.DriverLicenseNumber = driver.DriverLicenseNumber;
                vm.DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate;
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
            
            if (driver != null && id != driver.Id)
            {
                return NotFound();
            }

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
                            await _uow.DriverAndDriverLicenseCategories.RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(driver.Id);

                            foreach (var selectedDriverLicenseCategory in vm.DriverAndDriverLicenseCategories)
                            {
                                var driverAndDriverLicenseCategory = new DriverAndDriverLicenseCategory()
                                {
                                    DriverId = driver.Id,
                                    DriverLicenseCategoryId = selectedDriverLicenseCategory
                                };
                                 _uow.DriverAndDriverLicenseCategories.Add(driverAndDriverLicenseCategory);
                            }
                        }

                        driver.DriverLicenseNumber = vm.DriverLicenseNumber;
                        driver.DriverLicenseExpiryDate = vm.DriverLicenseExpiryDate;
                        driver.CityId = vm.CityId;
                        driver.Address = vm.Address;
                        _uow.Drivers.Update(driver);
                    }

                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver!.Id))
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

        // GET: AdminArea/Drivers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var vm = new DetailsDeleteDriverViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _uow.Drivers.FirstOrDefaultAsync(id.Value);
            if (driver == null)
            {
                return NotFound();
            }

            vm.PersonalIdentifier = driver.PersonalIdentifier;
            var driverLicenseCategoryNames =  
                await _uow.DriverAndDriverLicenseCategories.GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(driver.Id);
            vm.DriverLicenseCategoryNames = driverLicenseCategoryNames;
            vm.CityName = driver.City!.CityName;
            vm.Address = driver.Address;
            vm.DriverLicenseNumber = driver.DriverLicenseNumber;
            vm.DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate;
            
            return View(vm);
        }

        // POST: AdminArea/Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var driver = await _uow.Drivers.SingleOrDefaultAsync(d => d != null && d.Id.Equals(id));
            if (await _uow.Schedules.AnyAsync(d => driver != null && d != null && d.DriverId.Equals(driver.Id))
                || await _uow.Bookings.AnyAsync(d => driver != null && d != null && d.DriverId.Equals(driver.Id)))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }

            await _uow.DriverAndDriverLicenseCategories.RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(id);
           
            if (driver != null)
            {
                _uow.Drivers.Remove(driver);
                #warning ask how to remove appuser using uow
                /*var appUser = await _uow.Users
                    .SingleOrDefaultAsync(d => d.Id.Equals(driver.AppUserId));
                if (appUser != null) _uow.Users.Remove(appUser);*/
            }

            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverExists(Guid id)
        {
            return _uow.Drivers.Exists(id);
        }
        
    }
    
}
