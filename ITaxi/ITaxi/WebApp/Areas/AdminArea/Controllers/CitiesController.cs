#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CitiesController : Controller
    {
        private readonly AppDbContext _context;

        public CitiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/Cities
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Cities
                .Include(c => c.County);
            return View(await appDbContext.ToListAsync());
        }

        // GET: AdminArea/Cities/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vm = new DetailsDeleteCityViewModel();
            var city = await _context.Cities
                .Include(c => c.County)
                .SingleOrDefaultAsync(c => c.Id.Equals(id));
            if (city == null)
            {
                return NotFound();
            }

            vm.Id = city.Id;
            vm.CountyName = city.County!.CountyName;
            vm.CityName = city.CityName;

            return View(vm);
        }

        // GET: AdminArea/Cities/Create
        public async Task<IActionResult> Create()
        {
            var vm = new CreateEditCityViewModel();
            vm.Counties = new SelectList(
                await _context.Counties.OrderBy(c => c.CountyName)
                    .Select(c => new {c.Id, c.CountyName}).ToListAsync(),
                nameof(County.Id), nameof(County.CountyName));
            return View(vm);
        }

        // POST: AdminArea/Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditCityViewModel vm, City city)
        {
            if (ModelState.IsValid)
            {
                city.Id = Guid.NewGuid();
                city.CountyId = vm.CountyId;
                city.CityName = vm.CityName;
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(vm);
        }

        // GET: AdminArea/Cities/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new CreateEditCityViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            vm.Counties = new SelectList(await _context.Counties
                    .OrderBy(c => c.CountyName)
                    .Select(c => new {c.Id, c.CountyName}).ToListAsync(),
                nameof(County.Id), nameof(County.CountyName));
            vm.CityName = city.CityName;
            vm.CountyId = city.CountyId;
           
            return View(vm);
        }

        // POST: AdminArea/Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditCityViewModel vm)
        {
            
            var city = await _context.Cities.SingleAsync(c => c.Id.Equals(id));
            if (id != city.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    city.Id = id; 
                    city.CountyId = vm.CountyId;
                    city.CityName = vm.CityName;
                    city.UpdatedAt = DateTime.UtcNow;
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.Id))
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

        // GET: AdminArea/Cities/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var vm = new DetailsDeleteCityViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.County)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            vm.CityName = city.CityName;
            vm.CountyName = city.County!.CountyName;

            return View(vm);
        }

        // POST: AdminArea/Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var city = await _context.Cities.SingleOrDefaultAsync(c => c.Id.Equals(id));
            if (await _context.Admins.AnyAsync(c => c.CityId.Equals(id)) || 
                await _context.Bookings.AnyAsync(c => c.CityId.Equals(id)) ||
                await _context.Drivers.AnyAsync(c => c.CityId.Equals(id)))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }
            if (city != null) _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(Guid id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
    }
}
