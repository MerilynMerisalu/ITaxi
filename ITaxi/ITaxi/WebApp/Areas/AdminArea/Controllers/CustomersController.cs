#nullable enable

using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

[Area(nameof(AdminArea))]
public class CustomersController : Controller
{
    private readonly IAppBLL _appBLL;
    
    private readonly UserManager<AppUser> _userManager;

    public CustomersController(IAppBLL appBLL, UserManager<AppUser> userManager)
    {
        _appBLL = appBLL;
        _userManager = userManager;
        
    }

    // GET: AdminArea/Customers
    public async Task<IActionResult> Index()
    {
#warning Should this be a repo method
        var res = await _appBLL.Customers.GettingAllOrderedCustomersAsync();
        

        return View(res);
    }

    // GET: AdminArea/Customers/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteCustomerViewModel();
        if (id == null) return NotFound();

        var customer = await _appBLL.Customers.FirstOrDefaultAsync(id.Value);
        if (customer == null) return NotFound();


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
        vm.CreatedAt = customer.CreatedAt;
        vm.UpdatedBy = customer.UpdatedBy!;
        vm.UpdatedAt = customer.UpdatedAt;


        return View(vm);
    }

    // GET: AdminArea/Customers/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateCustomerViewModel();

        vm.DisabilityTypes = new SelectList(await _appBLL.DisabilityTypes.GetAllOrderedDisabilityTypesAsync(),
            nameof(DisabilityTypeDTO.Id), nameof(DisabilityTypeDTO.DisabilityTypeName));
        return View(vm);
    }

    // POST: AdminArea/Customers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCustomerViewModel vm)
    {
        var customer = new CustomerDTO();
        var appUser = new AppUser
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
            _appBLL.Customers.Add(customer);
            await _userManager.AddToRoleAsync(appUser, "Customer");
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        return View(vm);
    }

    // GET: AdminArea/Customers/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new EditCustomerViewModel();
        if (id == null) return NotFound();

        var customer = await _appBLL.Customers.FirstOrDefaultAsync(id.Value);
        if (customer == null) return NotFound();

        vm.DisabilityTypes = new SelectList(await _appBLL.DisabilityTypes
                .GetAllOrderedDisabilityTypesAsync()
            , nameof(DisabilityTypeDTO.Id),
            nameof(DisabilityTypeDTO.DisabilityTypeName));
        vm.DisabilityTypeId = customer.DisabilityTypeId;
        vm.FirstName = customer.AppUser!.FirstName;
        vm.LastName = customer.AppUser!.LastName;
        vm.Gender = customer.AppUser!.Gender;
        vm.DateOfBirth = customer!.AppUser.DateOfBirth;
        vm.IsActive = customer.AppUser!.IsActive;
        vm.PhoneNumber = customer.AppUser!.PhoneNumber;
        vm.Email = customer.AppUser!.Email;


        return View(vm);
    }

    // POST: AdminArea/Customers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditCustomerViewModel vm)
    {
        var customer = await _appBLL.Customers.FirstOrDefaultAsync(id, noTracking:true, noIncludes:true);
        
        if (customer == null || id != customer.Id) return NotFound();

        if (ModelState.IsValid)
        {
            customer.AppUser = null;
            var appUser = await _appBLL.AppUsers.GettingAppUserByAppUserIdAsync(customer.AppUserId);
            try
            {
                if (true)
                {
                    appUser.FirstName = vm.FirstName;
                    appUser.LastName = vm.LastName;
                    appUser.Gender = vm.Gender;
                    appUser.DateOfBirth = vm.DateOfBirth.ToUniversalTime();
                    appUser.Email = vm.Email;
                    appUser.PhoneNumber = vm.PhoneNumber;
                    appUser.IsActive = vm.IsActive;
                     _appBLL.AppUsers.Update(appUser);
                    await _appBLL.SaveChangesAsync();
                }

                customer.Id = id;
                customer.DisabilityTypeId = vm.DisabilityTypeId;
                customer.UpdatedBy = User.Identity!.Name!;
                customer.UpdatedAt = DateTime.Now.ToUniversalTime();
                _appBLL.Customers.Update(customer);
                
                await _appBLL.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/Customers/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteCustomerViewModel();
        if (id == null) return NotFound();

        var customer = await _appBLL.Customers.FirstOrDefaultAsync(id.Value);
        if (customer == null) return NotFound();

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
        vm.CreatedAt = customer.CreatedAt;
        vm.UpdatedBy = customer.UpdatedBy!;
        vm.UpdatedAt = customer.UpdatedAt;

        return View(vm);
    }

    // POST: AdminArea/Customers/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var customer = await _appBLL.Customers.FirstOrDefaultAsync(id);
        if (customer != null)
        {
            customer.AppUser = null;
                if (await _appBLL.Customers.HasBookingsAnyAsync(id))
                    return Content("Entity cannot be deleted because it has dependent entities!");

            var appUser = await _userManager.FindByIdAsync(customer!.AppUserId.ToString());
            if (appUser != null)
            {
                await _userManager.RemoveFromRoleAsync(appUser, "Customer");
                _appBLL.Customers.Remove(customer);

#warning temporarily solution
                var claims = await _userManager.GetClaimsAsync(appUser);
                await _userManager.RemoveClaimsAsync(appUser, claims);
                await _userManager.DeleteAsync(appUser);
            }
        }

        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CustomerExists(Guid id)
    {
        return _appBLL.Customers.Exists(id);
    }
}