#nullable disable
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
        private readonly AppDbContext _context;

        public DrivesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/Drives
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Drives
                .Include(d => d.Booking)
                .ThenInclude(d => d.Schedule)
                .Include(c => c.Booking)
                .ThenInclude(c => c.Customer)
                .ThenInclude(c => c.AppUser)
                .Include(c => c.Booking)
                .ThenInclude(c => c.City)
                .Include(b => b.Booking)
                .Include(v => v.Booking)
                .ThenInclude(v => v.Vehicle)
                .ThenInclude(v => v.VehicleType)
                .Include(v => v.Booking)
                .ThenInclude(v => v.Vehicle)
                .ThenInclude(v => v.VehicleMark)
                .Include(v => v.Booking)
                .ThenInclude(v => v.Vehicle)
                .ThenInclude(v => v.VehicleModel);
            return View(await appDbContext.ToListAsync());
        }

        // GET: AdminArea/Drives/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDriveViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var drive = await _context.Drives
                .Include(d => d.Driver)
                .Include(d => d.Booking)
                .ThenInclude(d => d.City)
                .Include(d => d.Booking)
                .ThenInclude(d => d.Schedule)
                .Include(d => d.Comment)
                .Include(d => d.Booking)
                .ThenInclude(d => d.Vehicle)
                .ThenInclude(v => v.VehicleMark)
                .Include(v => v.Booking)
                .ThenInclude(v => v.Vehicle)
                .ThenInclude(v => v.VehicleModel)
                .Include(v => v.Booking)
                .ThenInclude(v => v.VehicleType)
                .Include(c => c.Booking)
                .ThenInclude(c => c.Customer)
                .ThenInclude(c => c.AppUser)
                
                .SingleOrDefaultAsync(m => m.Id == id);
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
            vm.PickupDateAndTime = drive.Booking.PickUpDateAndTime.ToString("g");
            vm.StatusOfBooking = drive.Booking.StatusOfBooking;
            
            return View(vm);
        }

        /*
        // GET: AdminArea/Drives/Create
        public IActionResult Create()
        {
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address");
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
                _context.Add(drive);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", drive.DriverId);
            return View(drive);
        }

        // GET: AdminArea/Drives/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drive = await _context.Drives.FindAsync(id);
            if (drive == null)
            {
                return NotFound();
            }
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", drive.DriverId);
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
                    _context.Update(drive);
                    await _context.SaveChangesAsync();
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
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "Address", drive.DriverId);
            return View(drive);
        }

        // GET: AdminArea/Drives/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drive = await _context.Drives
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
            var drive = await _context.Drives.FindAsync(id);
            _context.Drives.Remove(drive);
            await _context.SaveChangesAsync();
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

            var drives = await _context.Drives.Include(b => b.Booking)
                .ThenInclude(b => b.Driver)
                .ThenInclude(d => d.AppUser)
                .Include(b => b.Booking)
                .ThenInclude(d => d.Schedule)
                .Include(b => b.Booking)
                .ThenInclude(v => v.Vehicle)
                .Include(v => v.Booking)
                .ThenInclude(v => v.Vehicle)
                .ThenInclude(v => v.VehicleMark)
                .Include(v => v.Booking)
                .ThenInclude(v => v.Vehicle)
                .ThenInclude(v => v.VehicleModel)
                .Include(v => v.Booking)
                .ThenInclude(v => v.VehicleType)
                .Include(d => d.Booking)
                .ThenInclude(c => c.Customer)
                .ThenInclude(c => c.AppUser)
                .Include(c => c.Booking)
                .ThenInclude(c => c.City)
                .Include(c => c.Comment)
                .Where(d => d.Booking!.PickUpDateAndTime.Date.Equals(search.Date)).ToListAsync();
            return View(nameof(Index), drives);
        }

        /// <summary>
        /// Generates a pdf view of drives
        /// </summary>
        /// <returns>A pdf of drives</returns>
        public async Task<IActionResult> Print()
        {
            
            var driver = await _context.Drivers.Select(d => d).FirstOrDefaultAsync();
           var drives = await _context.Drives.Include(d => d.Booking)
               .ThenInclude(d => d.Schedule)
                .Include(d => d.Booking)
                .ThenInclude(d => d.VehicleType)
                .Include(c => c.Booking)
                .ThenInclude(c => c.Customer)
                .ThenInclude(c => c.AppUser)
                .Include(c => c.Booking)
                .ThenInclude(c => c.City)
                .Include(d => d.Booking)
                .ThenInclude(d => d.Driver)
                .ThenInclude(d => d.AppUser)
                .Where(d => d.DriverId.Equals(driver.Id)).ToListAsync();

           return new ViewAsPdf("PrintDrives", drives);
        }
    }
}
