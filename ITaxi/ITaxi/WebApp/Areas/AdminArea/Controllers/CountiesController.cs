#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    
    public class CountiesController : Controller
    {
        private readonly AppDbContext _context;

        public CountiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/Counties
        public async Task<IActionResult> Index()
        {
            return View(await _context.Counties.ToListAsync());
        }

        // GET: AdminArea/Counties/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteCountyViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var county = await _context.Counties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (county == null)
            {
                return NotFound();
            }

            vm.CountyName = county.CountyName;
            vm.Id = county.Id;

            return View(vm);
        }

        // GET: AdminArea/Counties/Create
        public IActionResult Create()
        {
            var vm = new CreateEditCountyViewModel();
            return View(vm);
        }

        // POST: AdminArea/Counties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditCountyViewModel vm, County county)
        {
            if (ModelState.IsValid)
            {
                county.Id = Guid.NewGuid();
                county.CountyName = vm.CountyName;
                _context.Add(county);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: AdminArea/Counties/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new CreateEditCountyViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var county = await _context.Counties.FindAsync(id);
            
            if (county == null)
            {
                return NotFound();
            }

            vm.CountyName = county.CountyName;
            
            return View(vm);
        }

        // POST: AdminArea/Counties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditCountyViewModel vm)
        {
            var county = await _context.Counties.SingleAsync(c => c.Id.Equals(id));
            if (ModelState.IsValid)
            {
                if (county.Id.Equals(id))
                { try
                    {
                        county.CountyName = vm.CountyName;
                        _context.Update(county);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CountyExists(county.Id))
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

              
            }
            return View(vm);
        }

        // GET: AdminArea/Counties/Delete/5
            public async Task<IActionResult> Delete(Guid? id)
            {
                var vm = new DetailsDeleteCountyViewModel();
                if (id == null)
                {
                    return NotFound();
                }

                var county = await _context.Counties
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (county == null)
                {
                    return NotFound();
                }

                vm.CountyName = county.CountyName;

                return View(vm);
            }

            // POST: AdminArea/Counties/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(Guid id)
            {
                var county = await _context.Counties.SingleOrDefaultAsync(c => c.Id.Equals(id));
                if (await _context.Cities.AnyAsync(c => c.CountyId.Equals(county.Id)))
                {
                    return Content("Entity cannot be deleted because it has dependent entities!");
                }

                if (county != null) _context.Counties.Remove(county);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            private bool CountyExists(Guid id)
            {
                return _context.Counties.Any(e => e.Id == id);
            }
    }
}
