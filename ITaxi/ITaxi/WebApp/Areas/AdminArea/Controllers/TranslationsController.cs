using App.DAL.EF;
using Base.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.AdminArea.Controllers;

[Area("AdminArea")]
public class TranslationsController : Controller
{
    private readonly AppDbContext _context;

    public TranslationsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: AdminArea/Translations
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.Translations.Include(t => t.LangStr);
        return View(await appDbContext.ToListAsync());
    }

    // GET: AdminArea/Translations/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.Translations == null) return NotFound();

        var translation = await _context.Translations
            .Include(t => t.LangStr)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (translation == null) return NotFound();

        return View(translation);
    }

    // GET: AdminArea/Translations/Create
    public IActionResult Create()
    {
        ViewData["LangStrId"] = new SelectList(_context.LangStrings, "Id", "Id");
        return View();
    }

    // POST: AdminArea/Translations/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Culture,Value,LangStrId,Id")] Translation translation)
    {
        if (ModelState.IsValid)
        {
            translation.Id = Guid.NewGuid();
            _context.Add(translation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["LangStrId"] = new SelectList(_context.LangStrings, "Id", "Id", translation.LangStrId);
        return View(translation);
    }

    // GET: AdminArea/Translations/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null || _context.Translations == null) return NotFound();

        var translation = await _context.Translations.FindAsync(id);
        if (translation == null) return NotFound();
        ViewData["LangStrId"] = new SelectList(_context.LangStrings, "Id", "Id", translation.LangStrId);
        return View(translation);
    }

    // POST: AdminArea/Translations/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, [Bind("Culture,Value,LangStrId,Id")] Translation translation)
    {
        if (id != translation.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(translation);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TranslationExists(translation.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["LangStrId"] = new SelectList(_context.LangStrings, "Id", "Id", translation.LangStrId);
        return View(translation);
    }

    // GET: AdminArea/Translations/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.Translations == null) return NotFound();

        var translation = await _context.Translations
            .Include(t => t.LangStr)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (translation == null) return NotFound();

        return View(translation);
    }

    // POST: AdminArea/Translations/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (_context.Translations == null) return Problem("Entity set 'AppDbContext.Translations'  is null.");
        var translation = await _context.Translations.FindAsync(id);
        if (translation != null) _context.Translations.Remove(translation);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TranslationExists(Guid id)
    {
        return (_context.Translations?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}