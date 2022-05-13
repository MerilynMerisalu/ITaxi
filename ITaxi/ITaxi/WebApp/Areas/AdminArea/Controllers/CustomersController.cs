#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area(nameof(AdminArea))]
    public class CustomersController : Controller
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/Customers
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Customers
                .Include(c => c.AppUser)
                .Include(c => c.DisabilityType);
            return View(await appDbContext.ToListAsync());
        }

        // GET: AdminArea/Customers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteCustomerViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.AppUser)
                .Include(c => c.DisabilityType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            
            vm.Id = customer.Id;
            vm.LastAndFirstName = customer.AppUser!.LastAndFirstName;
            vm.DateOfBirth = customer.AppUser.DateOfBirth.Date.ToString("d");
            vm.Gender = customer.AppUser.Gender;
            vm.PhoneNumber = customer.AppUser.PhoneNumber;
            vm.Email = customer.AppUser.Email;
            vm.DisabilityTypeName = customer.DisabilityType!.DisabilityTypeName;

            return View(vm);
        }

        // GET: AdminArea/Customers/Create
        /*public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Email");
            ViewData["DisabilityTypeId"] = new SelectList(_context.DisabilityTypes, "Id", "DisabilityTypeName");
            return View();
        }

        // POST: AdminArea/Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppUserId,DisabilityTypeId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.Id = Guid.NewGuid();
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Email", customer.AppUserId);
            ViewData["DisabilityTypeId"] = new SelectList(_context.DisabilityTypes, "Id", "DisabilityTypeName", customer.DisabilityTypeId);
            return View(customer);
        }*/

        // GET: AdminArea/Customers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new EditCustomerViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.
                Include(c => c.DisabilityType)
                .SingleOrDefaultAsync(c => c.Id.Equals(id));
            if (customer == null)
            {
                return NotFound();
            }

            vm.DisabilityTypes = new SelectList(await _context.DisabilityTypes
                .Select(d => new {d.Id, d.DisabilityTypeName})
                .ToListAsync(), nameof(DisabilityType.Id),
                nameof(DisabilityType.DisabilityTypeName));
            vm.DisabilityTypeId = customer.DisabilityTypeId;
            
            
            return View(vm);
        }

        // POST: AdminArea/Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EditCustomerViewModel vm)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id.Equals(id));
            if (customer != null && id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (customer != null)
                    {
                        customer.Id = id;
                        customer.DisabilityTypeId = vm.DisabilityTypeId; 
                         _context.Update(customer);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (customer != null && !CustomerExists(customer.Id))
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

        // GET: AdminArea/Customers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var vm = new DetailsDeleteCustomerViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.AppUser)
                .Include(c => c.DisabilityType)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            vm.Id = customer.Id;
            vm.DisabilityTypeName = customer.DisabilityType!.DisabilityTypeName;
            return View(vm);
        }

        // POST: AdminArea/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.Id.Equals(id));
            if (await _context.Bookings.AnyAsync(c => c.CustomerId.Equals(customer.Id)))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }
            var appUser = await _context.Users.SingleAsync(a => a.Id.Equals(customer.AppUserId));
            if (customer != null) _context.Customers.Remove(customer);
            _context.Users.Remove(appUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(Guid id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
