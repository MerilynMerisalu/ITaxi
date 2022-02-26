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
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class AdminsController : Controller
    {
        private readonly AppDbContext _context;

        public AdminsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/Admins
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Admins
                .Include(a => a.AppUser)
                .Include(a => a.City);
            return View(await appDbContext.ToListAsync());
        }

        // GET: AdminArea/Admins/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteAdminViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .Include(a => a.AppUser)
                .Include(a => a.City)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            vm.Address = admin.Address;
            vm.City = admin.City;
            if (admin.PersonalIdentifier != null) vm.PersonalIdentifier = admin.PersonalIdentifier;
            vm.Id = admin.Id;

            return View(vm);
        }

        // GET: AdminArea/Admins/Create
        /*
        public IActionResult Create()
        {
            
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName");
            return View();
        }

        // POST: AdminArea/Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppUserId,PersonalIdentifier,CityId,Address,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                admin.Id = Guid.NewGuid();
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Email", admin.AppUserId);
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "CityName", admin.CityId);
            return View(admin);
        }
        */

        // GET: AdminArea/Admins/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new CreateEditAdminViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound();
            }

            vm.PersonalIdentifier = admin.PersonalIdentifier;
            vm.Address = admin.Address;
            vm.Cities = new SelectList(await _context.Cities
                    .OrderBy(c => c.CityName)
                    .Select(c => new {c.Id, c.CityName}).ToListAsync(),
                nameof(City.Id), nameof(City.CityName));
            return View(vm);
        }

        // POST: AdminArea/Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditAdminViewModel vm)
        {
            var admin = await _context.Admins.SingleAsync(a => a.Id.Equals(id));
            if (id != admin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    admin.Address = vm.Address;
                    admin.CityId = vm.CityId;
                    admin.PersonalIdentifier = vm.PersonalIdentifier;
                    admin.UpdatedAt = DateTime.UtcNow;
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminExists(admin.Id))
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

        // GET: AdminArea/Admins/Delete/5
        public async Task<IActionResult> Delete(Guid? id, DetailsDeleteAdminViewModel vm)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admin = await _context.Admins
                .Include(a => a.City)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            vm.Address = admin.Address;
            vm.City = admin.City;
            if (admin.PersonalIdentifier != null) vm.PersonalIdentifier = admin.PersonalIdentifier;

            return View(vm);
        }

        // POST: AdminArea/Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var admin = await _context.Admins.FindAsync(id);
            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminExists(Guid id)
        {
            return _context.Admins.Any(e => e.Id == id);
        }
    }
}
