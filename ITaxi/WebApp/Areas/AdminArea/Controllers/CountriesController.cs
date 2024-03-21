using System.Globalization;
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.Domain;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using WebApp.Areas.AdminArea.ViewModels;


namespace WebApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = nameof(Admin))]
    public class CountriesController : Controller
    {
        private readonly IAppBLL _appBLL;
        
        
        public CountriesController( IAppBLL appBLL)
        {
            
            _appBLL = appBLL;
        }

        // GET: AdminArea/Countries
        public async Task<IActionResult> Index()
        {
            var res = await _appBLL.Countries.GetAllCountriesOrderedByCountryNameAsync();
            return View(res);
        }

        // GET: AdminArea/Countries/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vm = new DetailsDeleteCountryViewModel();
            var country = await _appBLL.Countries.FirstOrDefaultAsync(id.Value);
            if (country == null)
            {
                return NotFound();
            }

            vm.Id = country.Id;
            vm.CountryName = country.CountryName;
            vm.CreatedBy = country.CreatedBy!;
            vm.CreatedAt = country.CreatedAt.ToLocalTime().ToString("g");
            vm.UpdatedBy = country.UpdatedBy!;
            vm.UpdatedAt = country.UpdatedAt.ToLocalTime().ToString("g");

            return View(vm);
        }

        // GET: AdminArea/Countries/Create
        public IActionResult Create()
        {
            var vm = new CreateEditCountryViewModel();
            return View(vm);
        }

        // POST: AdminArea/Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditCountryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var country = new CountryDTO();
                country.Id = Guid.NewGuid();
                country.CountryName = vm.CountryName;
                country.CreatedBy = User.GettingUserEmail();
                country.CreatedAt = DateTime.Now.ToUniversalTime();
                country.UpdatedBy = User.GettingUserEmail();
                country.UpdatedAt = DateTime.Now.ToUniversalTime();
                _appBLL.Countries.Add(country);
                await _appBLL.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: AdminArea/Countries/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var vm = new CreateEditCountryViewModel();
            var country = await _appBLL.Countries.FirstOrDefaultAsync(id.Value, noIncludes:true, noTracking:true);
            if (country == null)
            {
                return NotFound();
            }

            vm.Id = country.Id;
            vm.CountryName = country.CountryName;
            
            return View(vm);
        }

        // POST: AdminArea/Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditCountryViewModel vm)
        {
            var country = await _appBLL.Countries.FirstOrDefaultAsync(id, noIncludes:true);
            if (country != null && id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (country != null)
                    {
                        country.CountryName.SetTranslation(vm.CountryName); 
                        country.UpdatedBy = User.GettingUserEmail();
                        country.UpdatedAt = DateTime.Now.ToUniversalTime();
                        _appBLL.Countries.Update(country);
                        await _appBLL.SaveChangesAsync();
                    }

                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (country != null && !CountryExists(country.Id))
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

        // GET: AdminArea/Countries/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var vm = new DetailsDeleteCountryViewModel();
            var country = await _appBLL.Countries.FirstOrDefaultAsync(id.Value);


            if (country != null)
            {
                vm.Id = country.Id;
                vm.CountryName = country.CountryName;
                vm.CreatedBy = country.CreatedBy!;
                vm.CreatedAt = country.CreatedAt.ToLocalTime().ToString("g");
                vm.UpdatedBy = country.UpdatedBy!;
                vm.UpdatedAt = country.CreatedAt.ToLocalTime().ToString("g");
            }

            return View(vm);
        }

        // POST: AdminArea/Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            var country = await _appBLL.Countries.FirstOrDefaultAsync(id, noIncludes: true);
            if (await _appBLL.Countries.HasAnyCountiesAsync(id))
                return Content("Entity cannot be deleted because it has dependent entities!");

            await _appBLL.Countries.RemoveAsync(country!.Id);
            
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(Guid id)
        {
            return _appBLL.Countries.Exists(id);
        }
    }
}
