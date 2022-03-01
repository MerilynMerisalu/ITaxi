#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class DriversController : Controller
    {
        private readonly AppDbContext _context;

        public DriversController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/Drivers
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Drivers.Include(d => d.AppUser).Include(d => d.City);
            return View(await appDbContext.ToListAsync());
        }

        // GET: AdminArea/Drivers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteDriverViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .Include(d => d.AppUser)
                .Include(d => d.City)
                .SingleAsync(m => m.Id == id);

            var driverLicenseCategoryNames = await GettingDriverLicenseCategoryNamesAsync(driver.Id);
            vm.DriverLicenseCategoryNames = String.Join(", ", driverLicenseCategoryNames);
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName");
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
                _context.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Email", driver.AppUserId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", driver.CityId);
            return View(driver);
        }*/

        // GET: AdminArea/Drivers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .Include(c => c.DriverLicenseCategories)
                .ThenInclude(c => c.DriverLicenseCategory)
                .Include(c => c.City)
                .SingleAsync(c => c.Id.Equals(id));

            var vm = new CreateEditDriverViewModel();
            vm.DriverLicenseCategories= new SelectList(
                await _context
                    .DriverLicenseCategories.Include(d => d.Drivers)
                    .OrderBy(dl=> dl.DriverLicenseCategoryName)
                    .Select(d => new {d.Id, d.DriverLicenseCategoryName}).ToListAsync(),
                nameof(DriverLicenseCategory.Id), 
                nameof(DriverLicenseCategory.DriverLicenseCategoryName));
            vm.Cities = new SelectList(await _context.Cities.OrderBy(c => c.CityName)
                    .Select(c => new {c.Id, c.CityName}).ToListAsync(),
                nameof(City.Id), nameof(City.CityName));
            vm.Address = driver.Address;
            vm.CityId = driver.CityId;
            vm.PersonalIdentifier = driver.PersonalIdentifier;
            vm.DriverLicenseNumber = driver.DriverLicenseNumber;
            vm.DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate;
            
            return View(vm);
        }

        // POST: AdminArea/Drivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditDriverViewModel vm)
        {
            var driver = await _context.Drivers.SingleAsync(d => d.Id.Equals(id));
            
            if (id != driver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    driver.Id = id;
                    driver.PersonalIdentifier = vm.PersonalIdentifier;
                    if (vm.DriverAndDriverLicenseCategories != null)
                    {
                        await RemovingDriverAndDriverLicenseCategoriesAsync(id);

                        foreach (var selectedDriverLicenseCategory in vm.DriverAndDriverLicenseCategories)
                        {
                            var driverAndDriverLicenseCategory = new DriverAndDriverLicenseCategory()
                            {
                                DriverId = driver.Id,
                                DriverLicenseCategoryId = selectedDriverLicenseCategory
                            };
                            await _context.DriverAndDriverLicenseCategories.AddAsync(driverAndDriverLicenseCategory);
                            
                        }

                        
                    }
                    driver.DriverLicenseNumber = vm.DriverLicenseNumber;
                    driver.DriverLicenseExpiryDate = vm.DriverLicenseExpiryDate;
                    driver.CityId = vm.CityId;
                    driver.Address = vm.Address;
                    _context.Update(driver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.Id))
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
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .Include(d => d.AppUser)
                .Include(d => d.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (driver == null)
            {
                return NotFound();
            }

            return View(driver);
        }

        // POST: AdminArea/Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            await RemovingDriverAndDriverLicenseCategoriesAsync(id);
            _context.Drivers.Remove(driver);
            var appUser = await _context.Users
                .SingleOrDefaultAsync(d => d.Id.Equals(driver.AppUserId));
            _context.Users.Remove(appUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverExists(Guid id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }
        /// <summary>
        /// Deleting entries from an in-between table driverAndDriverLicenseCategory table
        /// </summary>
        /// <param name="id">Driver entity id</param>
        private async Task RemovingDriverAndDriverLicenseCategoriesAsync(Guid id)
        {
            var driverAndDriverLicenseCategories =
                await _context.DriverAndDriverLicenseCategories
                    .Where(dl => dl.DriverId.Equals(id))
                    .Select(dl => dl).ToListAsync();
            _context.DriverAndDriverLicenseCategories.RemoveRange(driverAndDriverLicenseCategories);

        }

        private async Task<ICollection<string>> GettingDriverLicenseCategoryNamesAsync(Guid id)
        {
           var driverLicenseCategoryNames = await _context.DriverAndDriverLicenseCategories
               .Include(c => c.DriverLicenseCategory)
               .Include(c => c.Driver)
                   .Where(i => i.DriverId.Equals(id))
               .Select(dl => dl.DriverLicenseCategory.DriverLicenseCategoryName)
               .ToListAsync();
           return driverLicenseCategoryNames;
        }
    }
    
}
