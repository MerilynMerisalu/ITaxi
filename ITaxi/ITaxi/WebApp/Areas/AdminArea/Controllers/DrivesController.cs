#nullable enable
using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using Rotativa.AspNetCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area(nameof(AdminArea))]
    public class DrivesController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public DrivesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: AdminArea/Drives
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Drives.GettingAllOrderedDrivesWithIncludesAsync());
        }

        // GET: AdminArea/Drives/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDriveViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var drive = await _uow.Drives
                .FirstOrDefaultAsync(id.Value);
            if (drive == null)
            {
                return NotFound();
            }

            vm.City = drive.Booking!.City!.CityName;
            vm.ShiftDurationTime = drive.Booking!.Schedule!.ShiftDurationTime;
            if (drive.Comment?.CommentText != null) vm.CommentText = drive.Comment.CommentText;
            
            vm.DestinationAddress = drive.Booking.DestinationAddress;
            vm.PickupAddress = drive.Booking.PickupAddress;
            vm.VehicleIdentifier = drive.Booking.Vehicle!.VehicleIdentifier;
            vm.VehicleType = drive.Booking.VehicleType!.VehicleTypeName;
            vm.HasAnAssistant = drive.Booking.HasAnAssistant.ToString();
            vm.NumberOfPassengers = drive.Booking.NumberOfPassengers.ToString();
            vm.LastAndFirstName = drive.Booking.Customer!.AppUser!.LastAndFirstName;
            vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToLongDateString() + " "
                + drive.Booking.PickUpDateAndTime.ToShortTimeString();
            vm.StatusOfBooking = drive.Booking.StatusOfBooking;
            
            return View(vm);
        }

        /*
        // GET: AdminArea/Drives/Create
        public IActionResult Create()
        {
            ViewData["DriverId"] = new SelectList(_uow.Drivers, "Id", "Address");
            return View();
        }

        // POST: AdminArea/Drives/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DriverId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Drive drive)
        {
            if (ModelState.IsValid)
            {
                drive.Id = Guid.NewGuid();
                _uow.Add(drive);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverId"] = new SelectList(_uow.Drivers, "Id", "Address", drive.DriverId);
            return View(drive);
        }

        // GET: AdminArea/Drives/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drive = await _uow.Drives.FindAsync(id);
            if (drive == null)
            {
                return NotFound();
            }
            ViewData["DriverId"] = new SelectList(_uow.Drivers, "Id", "Address", drive.DriverId);
            return View(drive);
        }

        // POST: AdminArea/Drives/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DriverId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Drive drive)
        {
            if (id != drive.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _uow.Update(drive);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriveExists(drive.Id))
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
            ViewData["DriverId"] = new SelectList(_uow.Drivers, "Id", "Address", drive.DriverId);
            return View(drive);
        }

        // GET: AdminArea/Drives/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drive = await _uow.Drives
                .Include(d => d.Driver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drive == null)
            {
                return NotFound();
            }

            return View(drive);
        }

        // POST: AdminArea/Drives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var drive = await _uow.Drives.FindAsync(id);
            _uow.Drives.Remove(drive);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        */

        /// <summary>
        /// Search drives by inserted date 
        /// </summary>
        /// <param name="search">date</param>
        /// <returns>An index view with search results</returns>
        [HttpPost]
        public async Task<IActionResult> SearchByDateAsync([FromForm] DateTime search)
        {

            var drives = await _uow.Drives.SearchByDateAsync(search);
            return View(nameof(Index), drives);
        }

        /// <summary>
        /// Generates a pdf view of drives
        /// </summary>
        /// <returns>A pdf of drives</returns>
        public async Task<IActionResult> Print()
        {
            
            var driver = await _uow.Drivers.FirstAsync();
            if (driver != null)
            {
                var drives = await _uow.Drives.PrintAsync(driver.Id);

                return new ViewAsPdf("PrintDrives", drives);
            }

            return new ViewAsPdf("PrintDrives");
        }
    }
    
}
