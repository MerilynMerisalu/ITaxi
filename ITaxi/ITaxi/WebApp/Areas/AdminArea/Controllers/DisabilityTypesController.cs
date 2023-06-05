#nullable enable
using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;
using DisabilityTypeDTO = App.BLL.DTO.AdminArea.DisabilityTypeDTO;

namespace WebApp.Areas.AdminArea.Controllers;

/// <summary>
/// Admin area disability types controller
/// </summary>
[Area(nameof(AdminArea))]
public class DisabilityTypesController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Admin area disability types controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public DisabilityTypesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/DisabilityTypes
    /// <summary>
    /// Admin area disability types index
    /// </summary>
    /// <returns>View with data</returns>
    public async Task<IActionResult> Index()
    {
        var res = await _appBLL.DisabilityTypes.GetAllOrderedDisabilityTypesAsync();
        
        return View(res);
    }

    // GET: AdminArea/DisabilityTypes/Details/5
    /// <summary>
    /// Admin area disability type details 
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View model</returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteDisabilityTypeViewModel();
        if (id == null) return NotFound();

        var disabilityType = await _appBLL.DisabilityTypes
            .FirstOrDefaultAsync(id.Value);
        if (disabilityType == null) return NotFound();

        vm.Id = disabilityType.Id;
        vm.DisabilityType = disabilityType.DisabilityTypeName;
        vm.CreatedBy = disabilityType.CreatedBy!;
        vm.CreatedAt = disabilityType.CreatedAt;
        vm.UpdatedBy = disabilityType.UpdatedBy!;
        vm.UpdatedAt = disabilityType.UpdatedAt;

        return View(vm);
    }

    // GET: AdminArea/DisabilityTypes/Create
    /// <summary>
    /// Admin area disability type create GET method
    /// </summary>
    /// <returns>View model</returns>
    public IActionResult Create()
    {
        var vm = new CreateEditDisabilityTypeViewModel();
        return View(vm);
    }

    // POST: AdminArea/DisabilityTypes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area disability type create POST method
    /// </summary>
    /// <param name="vm">View model</param>
    /// <returns>View model</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditDisabilityTypeViewModel vm)
    {
        var disabilityType = new DisabilityTypeDTO();
        if (ModelState.IsValid)
        {
            disabilityType.Id = Guid.NewGuid();
            disabilityType.DisabilityTypeName = vm.DisabilityTypeName;
            disabilityType.CreatedBy = User.Identity!.Name;
            disabilityType.CreatedAt = DateTime.Now;
            _appBLL.DisabilityTypes.Add(disabilityType);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/DisabilityTypes/Edit/5
    /// <summary>
    /// Admin area disability type edit GET method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View model</returns>
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditDisabilityTypeViewModel();
        if (id == null) return NotFound();

        var disabilityType = await _appBLL.DisabilityTypes.FirstOrDefaultAsync(id.Value);
        if (disabilityType == null) return NotFound();
        
        vm.DisabilityTypeName = disabilityType.DisabilityTypeName;
        return View(vm);
    }

    // POST: AdminArea/DisabilityTypes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area disability type edit POST method
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View model</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateEditDisabilityTypeViewModel vm)
    {
        var disabilityType = await _appBLL.DisabilityTypes.FirstOrDefaultAsync(id);
        if (disabilityType != null && id != disabilityType.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (disabilityType != null)
                {
                    disabilityType.Id = id;
                    disabilityType.DisabilityTypeName.SetTranslation(vm.DisabilityTypeName);
                    disabilityType.UpdatedBy = User.Identity!.Name;
                    disabilityType.UpdatedAt = DateTime.Now;
                    _appBLL.DisabilityTypes.Update(disabilityType);
                    await _appBLL.SaveChangesAsync();
                }
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (disabilityType != null && !DisabilityTypeExists(disabilityType.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/DisabilityTypes/Delete/5
    /// <summary>
    /// Admin area disability type delete GET method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View model</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteDisabilityTypeViewModel();
        if (id == null) return NotFound();

        var disabilityType = await _appBLL.DisabilityTypes
            .FirstOrDefaultAsync(id.Value);
        if (disabilityType == null) return NotFound();

        vm.DisabilityType = disabilityType.DisabilityTypeName;
        vm.CreatedBy = disabilityType.CreatedBy!;
        vm.CreatedAt = disabilityType.CreatedAt;
        vm.UpdatedBy = disabilityType.UpdatedBy!;
        vm.UpdatedAt = disabilityType.UpdatedAt;
        
        return View(vm);
    }

    // POST: AdminArea/DisabilityTypes/Delete/5
    /// <summary>
    /// Admin area disability type delete POST method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index page</returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var disabilityType = await _appBLL.DisabilityTypes
            .FirstOrDefaultAsync(id);
        if (await _appBLL.DisabilityTypes.HasAnyCustomersAsync(id))
            return Content("Entity cannot be deleted because it has dependent entities!");

        if (disabilityType != null) await _appBLL.DisabilityTypes.RemoveAsync(disabilityType.Id);
        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DisabilityTypeExists(Guid id)
    {
        return _appBLL.DisabilityTypes.Exists(id);
    }
}