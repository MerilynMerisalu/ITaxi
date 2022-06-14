#nullable enable
using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area(nameof(AdminArea))]
    public class CustomersController : Controller
    {
        private readonly IAppUnitOfWork _uow;

        public CustomersController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: AdminArea/Customers
        public async Task<IActionResult> Index()
        {
            
            return View(await _uow.Customers.GettingAllOrderedCustomersAsync());
        }

        // GET: AdminArea/Customers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteCustomerViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _uow.Customers.FirstOrDefaultAsync(id.Value);
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
            ViewData["AppUserId"] = new SelectList(_uow.Users, "Id", "Email");
            ViewData["DisabilityTypeId"] = new SelectList(_uow.DisabilityTypes, "Id", "DisabilityTypeName");
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
                _uow.Add(customer);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_uow.Users, "Id", "Email", customer.AppUserId);
            ViewData["DisabilityTypeId"] = new SelectList(_uow.DisabilityTypes, "Id", "DisabilityTypeName", customer.DisabilityTypeId);
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

            var customer = await _uow.Customers.FirstOrDefaultAsync(id.Value);
            if (customer == null)
            {
                return NotFound();
            }

            vm.DisabilityTypes = new SelectList(await _uow.DisabilityTypes
                .GetAllOrderedDisabilityTypesAsync()
                , nameof(DisabilityType.Id),
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
            var customer = await _uow.Customers.FirstOrDefaultAsync(id);
            if (customer == null || id == customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (true)
                    {
                        customer.Id = id;
                        customer.DisabilityTypeId = vm.DisabilityTypeId; 
                         _uow.Customers.Update(customer);
                    }

                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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

            var customer = await _uow.Customers.FirstOrDefaultAsync(id.Value);
            if (customer == null)
            {
                return NotFound();
            }

            vm.Id = customer.Id;
            vm.LastAndFirstName = customer.AppUser!.LastAndFirstName;
            vm.Email = customer.AppUser!.Email;
            vm.Gender = customer.AppUser!.Gender;
            vm.DateOfBirth = customer.AppUser!.DateOfBirth.ToString("d");
            vm.PhoneNumber = customer.AppUser.PhoneNumber;
            vm.DisabilityTypeName = customer.DisabilityType!.DisabilityTypeName;
            return View(vm);
        }

        // POST: AdminArea/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var customer = await _uow.Customers.FirstOrDefaultAsync(id);
            if (await _uow.Bookings.AnyAsync(c => customer != null && c != null && c.CustomerId.Equals(customer.Id)))
            {
                return Content("Entity cannot be deleted because it has dependent entities!");
            }
            //var appUser = await _uow.Users.SingleAsync(a => a.Id.Equals(customer.AppUserId));
            if (customer != null) _uow.Customers.Remove(customer);
            #warning Ask how to delete an user when using uow 
            //_uow.Users.Remove(appUser);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(Guid id)
        {
            return _uow.Customers.Exists(id);
        }
    }
}
