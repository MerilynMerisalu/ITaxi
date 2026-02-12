using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using App.Contracts.DAL.IAppRepositories;
using Microsoft.AspNetCore.Authorization;
using WebApp.Areas.AdminArea.ViewModels;
using System.Globalization;
using Base.Extensions;
using App.DAL.EF.Repositories;
using Base.Contracts;
using AutoMapper;
using App.Contracts.BLL;
using App.BLL.DTO.AdminArea;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "Admin")]
    public class ExtraServicesController : Controller
    {
        private readonly IAppBLL _appBLL;
        private readonly IMapper _mapper;
        
        public ExtraServicesController(IAppBLL appBLL, IMapper mapper)
        {
            _appBLL = appBLL;
            _mapper = mapper;
        }

        // GET: AdminArea/ExtraServices
        public async Task<IActionResult> Index()
        {
            var res = await _appBLL.ExtraServices.GetAllExtraServicesOrderedByNameAsync();
            return View(res);
        }

        // GET: AdminArea/ExtraServices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteExtraServiceViewModel();
           
            if (id == null)
            {
                return NotFound();
            }
            

            var extraService = await _appBLL.ExtraServices
                .FirstOrDefaultAsync(id.Value);
            if (extraService == null)
            {
                return NotFound();
            }
            vm.Id = extraService.Id;
            vm.ExtraServiceName = extraService.ExtraServiceName;
            vm.Description = extraService.Description;
            vm.Price = extraService.Price.ToString("C", CultureInfo.CurrentUICulture);
            vm.Type = extraService.Type.ToString();
            vm.CreatedBy = extraService.CreatedBy;
            vm.CreatedAt = extraService.CreatedAt;
            vm.UpdatedBy = extraService.UpdatedBy;
            vm.UpdatedAt = extraService.UpdatedAt;
            return View(vm);
        }

        // GET: AdminArea/ExtraServices/Create
        public IActionResult Create()
        {
            var vm = new CreateEditExtraServiceViewModel();
            return View(vm);
        }

        // POST: AdminArea/ExtraServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditExtraServiceViewModel vm )
        {
            if (ModelState.IsValid)
            {
                var extraService = new ExtraServiceDTO()
                {
                    Id = Guid.NewGuid(),
                    ExtraServiceName = vm.ExtraServiceName,
                    Description = vm.Description,
                    Price = vm.Price,
                    Type = vm.ExtraServiceType,
                    CreatedBy = User.GettingUserEmail(),
                    CreatedAt = DateTime.Now.ToUniversalTime(),
                    UpdatedBy = User.GettingUserEmail(),
                    UpdatedAt = DateTime.Now.ToUniversalTime(),
                };
                _appBLL.ExtraServices.Add(extraService);
                await _appBLL.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }

        // GET: AdminArea/ExtraServices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new CreateEditExtraServiceViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var extraService = await _appBLL.ExtraServices.GetExtraServiceByIdWithIncludesAsync(id.Value, roleName: null);
            if (extraService == null)
            {
                return NotFound();
            }
            vm.Id = extraService.Id;
            vm.ExtraServiceName = extraService.ExtraServiceName;
            vm.Description = extraService.Description;
            vm.Price = (decimal)extraService.Price;
            vm.ExtraServiceType = extraService.Type;
            return View(vm);
        }

        // POST: AdminArea/ExtraServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditExtraServiceViewModel vm)
        {
            var extraService = await _appBLL.ExtraServices.GetExtraServiceByIdWithoutIncludesAsync(id, roleName: null);
            if (extraService == null || extraService.Id != id )
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    extraService.Id = id;
                    extraService.ExtraServiceName = vm.ExtraServiceName;
                    extraService.Description = vm.Description;
                    extraService.Price = vm.Price;
                    extraService.Type = vm.ExtraServiceType;
                    extraService.UpdatedBy = User.GettingUserEmail();
                    extraService.UpdatedAt = DateTime.UtcNow;
                    _appBLL.ExtraServices.Update(extraService);
                    await _appBLL.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ExtraServiceExists(extraService.Id))
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
            return View(extraService);
        }

        // GET: AdminArea/ExtraServices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var vm = new DetailsDeleteExtraServiceViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var extraService = await _appBLL.ExtraServices
                .FirstOrDefaultAsync(id.Value);
            if (extraService == null)
            {
                return NotFound();
            }
            vm.Id = extraService.Id;
            vm.ExtraServiceName = extraService.ExtraServiceName;
            vm.Description = extraService.Description;
            vm.Price = extraService.Price.ToString("C", CultureInfo.CurrentUICulture);
            vm.Type = extraService.Type.ToString();

            return View(vm);
        }

        // POST: AdminArea/ExtraServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var extraService = await _appBLL.ExtraServices.GetExtraServiceByIdWithoutIncludesAsync(id);
            if (extraService != null)
            {
                _appBLL.ExtraServices.Remove(extraService);
            }

            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task< bool> ExtraServiceExists(Guid id)
        {
            return await _appBLL.ExtraServices.ExistsAsync(id);
        }
    }
}
