using App.DAL.EF;
using Base.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.AdminArea.Controllers;

[Area("AdminArea")]
public class LangStringsController : Controller
{
    private readonly AppDbContext _context;

    public LangStringsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: AdminArea/LangStrings
    public async Task<IActionResult> Index()
    {
        return _context.LangStrings != null
            ? View(await _context.LangStrings.ToListAsync())
            : Problem("Entity set 'AppDbContext.LangStrings'  is null.");
    }

    // GET: AdminArea/LangStrings/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.LangStrings == null) return NotFound();

        var langStr = await _context.LangStrings
            .FirstOrDefaultAsync(m => m.Id == id);
        if (langStr == null) return NotFound();

        return View(langStr);
    }

    // GET: AdminArea/LangStrings/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: AdminArea/LangStrings/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id")] LangStr langStr)
    {
        if (ModelState.IsValid)
        {
            langStr.Id = Guid.NewGuid();
            _context.Add(langStr);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(langStr);
    }

    // GET: AdminArea/LangStrings/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.LangStrings == null) return NotFound();

        var langStr = await _context.LangStrings.FindAsync(id);
        if (langStr == null) return NotFound();
        return View(langStr);
    }

    // POST: AdminArea/LangStrings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Id")] LangStr langStr)
    {
        if (id != langStr.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(langStr);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LangStrExists(langStr.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(langStr);
    }

    // GET: AdminArea/LangStrings/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.LangStrings == null) return NotFound();

        var langStr = await _context.LangStrings
            .FirstOrDefaultAsync(m => m.Id == id);
        if (langStr == null) return NotFound();

        return View(langStr);
    }

    // POST: AdminArea/LangStrings/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.LangStrings == null) return Problem("Entity set 'AppDbContext.LangStrings'  is null.");
        var langStr = await _context.LangStrings.FindAsync(id);
        if (langStr != null) _context.LangStrings.Remove(langStr);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool LangStrExists(Guid id)
    {
        return (_context.LangStrings?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}