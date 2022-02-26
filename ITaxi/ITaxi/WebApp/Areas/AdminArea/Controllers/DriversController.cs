#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

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

        // GET: AdminArea/Drivers/Create
        public IActionResult Create()
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
        }

        // GET: AdminArea/Drivers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Email", driver.AppUserId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", driver.CityId);
            return View(driver);
        }

        // POST: AdminArea/Drivers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AppUserId,PersonalIdentifier,DriverLicenseNumber,DriverLicenseExpiryDate,CityId,Address,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Driver driver)
        {
            if (id != driver.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Email", driver.AppUserId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", driver.CityId);
            return View(driver);
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
            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DriverExists(Guid id)
        {
            return _context.Drivers.Any(e => e.Id == id);
        }
    }
}
