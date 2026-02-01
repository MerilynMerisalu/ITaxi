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
    public class ExtraServicesController : Controller
    {
        private readonly AppDbContext _context;

        public ExtraServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/ExtraServices
        public async Task<IActionResult> Index()
        {
            return View(await _context.ExtraServices.ToListAsync());
        }

        // GET: AdminArea/ExtraServices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var extraService = await _context.ExtraServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (extraService == null)
            {
                return NotFound();
            }

            return View(extraService);
        }

        // GET: AdminArea/ExtraServices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminArea/ExtraServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,Price,ExtraServiceType,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,DeletedBy,DeletedAt,Id,IsDeleted,IsIgnored")] ExtraService extraService)
        {
            if (ModelState.IsValid)
            {
                extraService.Id = Guid.NewGuid();
                _context.Add(extraService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(extraService);
        }

        // GET: AdminArea/ExtraServices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var extraService = await _context.ExtraServices.FindAsync(id);
            if (extraService == null)
            {
                return NotFound();
            }
            return View(extraService);
        }

        // POST: AdminArea/ExtraServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Description,Price,ExtraServiceType,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,DeletedBy,DeletedAt,Id,IsDeleted,IsIgnored")] ExtraService extraService)
        {
            if (id != extraService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(extraService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExtraServiceExists(extraService.Id))
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
            return View(extraService);
        }

        // GET: AdminArea/ExtraServices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var extraService = await _context.ExtraServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (extraService == null)
            {
                return NotFound();
            }

            return View(extraService);
        }

        // POST: AdminArea/ExtraServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var extraService = await _context.ExtraServices.FindAsync(id);
            if (extraService != null)
            {
                _context.ExtraServices.Remove(extraService);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExtraServiceExists(Guid id)
        {
            return _context.ExtraServices.Any(e => e.Id == id);
        }
    }
}
