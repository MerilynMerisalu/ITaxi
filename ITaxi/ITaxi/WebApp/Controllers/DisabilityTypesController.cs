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

namespace WebApp.Controllers
{
    public class DisabilityTypesController : Controller
    {
        private readonly AppDbContext _context;

        public DisabilityTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: DisabilityTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.DisabilityTypes.ToListAsync());
        }

        // GET: DisabilityTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disabilityType = await _context.DisabilityTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (disabilityType == null)
            {
                return NotFound();
            }

            return View(disabilityType);
        }

        // GET: DisabilityTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DisabilityTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DisabilityTypeId,DisabilityTypeName,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] DisabilityType disabilityType)
        {
            if (ModelState.IsValid)
            {
                disabilityType.Id = Guid.NewGuid();
                _context.Add(disabilityType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(disabilityType);
        }

        // GET: DisabilityTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disabilityType = await _context.DisabilityTypes.FindAsync(id);
            if (disabilityType == null)
            {
                return NotFound();
            }
            return View(disabilityType);
        }

        // POST: DisabilityTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DisabilityTypeId,DisabilityTypeName,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] DisabilityType disabilityType)
        {
            if (id != disabilityType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(disabilityType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DisabilityTypeExists(disabilityType.Id))
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
            return View(disabilityType);
        }

        // GET: DisabilityTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var disabilityType = await _context.DisabilityTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (disabilityType == null)
            {
                return NotFound();
            }

            return View(disabilityType);
        }

        // POST: DisabilityTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var disabilityType = await _context.DisabilityTypes.FindAsync(id);
            _context.DisabilityTypes.Remove(disabilityType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DisabilityTypeExists(Guid id)
        {
            return _context.DisabilityTypes.Any(e => e.Id == id);
        }
    }
}
