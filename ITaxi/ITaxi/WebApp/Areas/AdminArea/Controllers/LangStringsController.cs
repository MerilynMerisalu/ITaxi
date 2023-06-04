using App.DAL.EF;
using Base.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Controller for translations
/// </summary>
[Area("AdminArea")]
public class LangStringsController : Controller
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor for lang string controller
    /// </summary>
    /// <param name="context">DB context</param>
    public LangStringsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: AdminArea/LangStrings
    /// <summary>
    /// Lang string index
    /// </summary>
    /// <returns>All lang strings</returns>
    public async Task<IActionResult> Index()
    {
        return true
            ? View(await _context.LangStrings.ToListAsync())
            : Problem("Entity set 'AppDbContext.LangStrings'  is null.");
    }

    // GET: AdminArea/LangStrings/Details/5
    /// <summary>
    /// Lang string details method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Lang string str</returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null || _context.LangStrings == null) return NotFound();

        var langStr = await _context.LangStrings
            .FirstOrDefaultAsync(m => m.Id == id);
        if (langStr == null) return NotFound();

        return View(langStr);
    }

    // GET: AdminArea/LangStrings/Create
    /// <summary>
    /// Lang string GET create method
    /// </summary>
    /// <returns>Empty view</returns>
    public IActionResult Create()
    {
        return View();
    }

    // POST: AdminArea/LangStrings/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Lang string POST create method
    /// </summary>
    /// <param name="langStr">Adding lang string</param>
    /// <returns>New lang string</returns>
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
    /// <summary>
    /// Lang string GET edit method
    /// </summary>
    /// <param name="id">Lang string id</param>
    /// <returns>Lang string</returns>
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
    /// <summary>
    /// Lang string POST edit method
    /// </summary>
    /// <param name="id">Lang string id</param>
    /// <param name="langStr">Lang str for the new lang string</param>
    /// <returns>Lang string</returns>
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
    /// <summary>
    /// Lang string GET delete method
    /// </summary>
    /// <param name="id">Lang string id</param>
    /// <returns>Deleted lang string</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null || _context.LangStrings == null) return NotFound();

        var langStr = await _context.LangStrings
            .FirstOrDefaultAsync(m => m.Id == id);
        if (langStr == null) return NotFound();

        return View(langStr);
    }

    // POST: AdminArea/LangStrings/Delete/5
    /// <summary>
    /// Lang string POST delete method
    /// </summary>
    /// <param name="id">Lang string id</param>
    /// <returns>Redirects the user to the index page</returns>
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