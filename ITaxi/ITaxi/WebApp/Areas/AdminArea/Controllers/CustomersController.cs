#nullable enable
using App.Contracts.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area(nameof(AdminArea))]
    public class CustomersController : Controller
    {
        private readonly IAppUnitOfWork _uow;
        private readonly UserManager<AppUser> _userManager;
        public CustomersController(IAppUnitOfWork uow, UserManager<AppUser> userManager)
        {
            _uow = uow;
            _userManager = userManager;
        }

        // GET: AdminArea/Customers
        public async Task<IActionResult> Index()
        {
#warning Should this be a repo method
            var res = await _uow.Customers.GettingAllOrderedCustomersAsync();
            foreach (var customer in res)
            {
                if (customer != null)
                {
                    customer.CreatedAt = customer.CreatedAt.ToLocalTime();
                    customer.UpdatedAt = customer.UpdatedAt.ToLocalTime();
                }
            }
            return View(res);
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
            vm.FirstName = customer.AppUser!.FirstName;
            vm.LastName = customer.AppUser!.LastName;
            vm.LastAndFirstName = customer.AppUser!.LastAndFirstName;
            vm.DateOfBirth = customer.AppUser.DateOfBirth.Date.ToString("d");
            vm.Gender = customer.AppUser.Gender;
            vm.PhoneNumber = customer.AppUser.PhoneNumber;
            vm.Email = customer.AppUser.Email;
            vm.DisabilityTypeName = customer.DisabilityType!.DisabilityTypeName;
            vm.CreatedBy = customer.CreatedBy!;
            vm.CreatedAt = customer.CreatedAt.ToLocalTime().ToString("G");
            vm.UpdatedBy = customer.UpdatedBy!;
            vm.UpdatedAt = customer.UpdatedAt.ToLocalTime().ToString("G");
            

            return View(vm);
        }

        // GET: AdminArea/Customers/Create
        public async Task<IActionResult> Create()
        {
            var vm = new CreateCustomerViewModel();
            
            vm.DisabilityTypes = new SelectList(await _uow.DisabilityTypes.GetAllOrderedDisabilityTypesAsync(),
                nameof(DisabilityType.Id), nameof(DisabilityType.DisabilityTypeName));
            return View(vm);
        }

        // POST: AdminArea/Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerViewModel vm)
        {
            var customer = new Customer();
            var appUser = new AppUser()
            {
                Id = Guid.NewGuid(),
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Gender = vm.Gender,
                DateOfBirth = DateTime.Parse(vm.DateOfBirth).ToUniversalTime(),
                PhoneNumber = vm.PhoneNumber,
                PhoneNumberConfirmed = true,
                Email = vm.Email,
                EmailConfirmed = true

            };
            await _userManager.CreateAsync(appUser, vm.Password);
            
            
            if (ModelState.IsValid)
            {
                customer.Id = Guid.NewGuid();
                customer.AppUserId = appUser.Id;
                customer.DisabilityTypeId = vm.DisabilityTypeId;
                _uow.Customers.Add(customer);
                await _userManager.AddToRoleAsync(appUser, nameof(Customer));
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            
            return View(vm);
        }

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
            vm.FirstName = customer.AppUser!.FirstName;
            vm.LastName = customer.AppUser!.LastName;
            vm.LastAndFirstName = customer.AppUser!.LastAndFirstName;
            vm.Email = customer.AppUser!.Email;
            vm.Gender = customer.AppUser!.Gender;
            vm.DateOfBirth = customer.AppUser!.DateOfBirth.ToString("d");
            vm.PhoneNumber = customer.AppUser.PhoneNumber;
            vm.DisabilityTypeName = customer.DisabilityType!.DisabilityTypeName;
            vm.CreatedBy = customer.CreatedBy!;
            vm.CreatedAt = customer.CreatedAt.ToLocalTime().ToString("G");
            vm.UpdatedBy = customer.UpdatedBy!;
            vm.UpdatedAt = customer.UpdatedAt.ToLocalTime().ToString("G");

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
