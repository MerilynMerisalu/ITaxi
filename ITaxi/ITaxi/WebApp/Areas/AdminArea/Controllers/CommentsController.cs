#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers;

[Authorize(Roles = nameof(Admin))]
[Area(nameof(AdminArea))]
public class CommentsController : Controller
{
    private readonly IAppUnitOfWork _uow;

    public CommentsController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: AdminArea/Comments
    public async Task<IActionResult> Index()
    {
        var roleName = User.GettingUserRoleName();
        var res = await _uow.Comments.GettingAllOrderedCommentsWithIncludesAsync(null, roleName);
        
        foreach (var comment in res)
        {
            comment.Drive!.Booking!.PickUpDateAndTime = comment.Drive.Booking!.PickUpDateAndTime.ToLocalTime();
            comment.CreatedAt = comment.CreatedAt.ToLocalTime();
            comment.UpdatedAt = comment.UpdatedAt.ToLocalTime();
        }
        return View(res);
    }

    // GET: AdminArea/Comments/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteCommentViewModel();
        if (id == null) return NotFound();

        var roleName = User.GettingUserRoleName();

        var comment = await _uow.Comments.GettingTheFirstCommentAsync(id.Value, null, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.Drive = comment.Drive!.Booking!.PickUpDateAndTime.ToLocalTime().ToString("G");
        vm.CustomerName = comment.Drive!.Booking!.Customer!.AppUser!.LastAndFirstName;
        vm.DriverName = comment.Drive!.Booking!.Driver!.AppUser!.LastAndFirstName;
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;
        vm.CreatedAt = comment.CreatedAt.ToLocalTime().ToString("G");
        vm.CreatedBy = comment.CreatedBy!;
        vm.UpdatedAt = comment.UpdatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = comment.UpdatedBy!;


        return View(vm);
    }

    // GET: AdminArea/Comments/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateEditCommentViewModel();

        var roleName = User.GettingUserRoleName();

        vm.Drives = new SelectList(await _uow.Drives.GettingDrivesWithoutCommentAsync(null, roleName),
            nameof(Drive.Id), nameof(Drive.DriveDescription));

        return View(vm);
    }

    // POST: AdminArea/Comments/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateEditCommentViewModel vm)
    {
        var roleName = User.GettingUserRoleName();
        var comment = new Comment();
        if (ModelState.IsValid)
        {
            comment.Id = Guid.NewGuid();
            comment.DriveId = vm.DriveId;
            comment.CommentText = vm.CommentText;
            comment.CreatedBy = User.Identity!.Name;
            comment.CreatedAt = DateTime.Now.ToUniversalTime();

            _uow.Comments.Add(comment);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.Drives = new SelectList(await _uow.Drives.GettingDrivesWithoutCommentAsync(null, roleName),
            nameof(Drive.Id),
            nameof(Booking.DriveTime));


        return View(vm);
    }

    // GET: AdminArea/Comments/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var roleName = User.GettingUserRoleName();
        var vm = new CreateEditCommentViewModel();
        if (id == null) return NotFound();

        var comment = await _uow.Comments.GettingTheFirstCommentAsync(id.Value, null, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;

        vm.Drives = new SelectList(await _uow.Drives.GettingAllDrivesForCommentsAsync(null, roleName),
            nameof(Drive.Id),
            nameof(Drive.DriveDescription));
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;
        vm.DriveId = comment.Drive!.Id;


        return View(vm);
    }

    // POST: AdminArea/Comments/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateEditCommentViewModel vm)
    {
        var roleName = User.GettingUserRoleName();
        var comment = await _uow.Comments.GettingTheFirstCommentAsync(id, null, roleName);
        if (comment != null && id != comment.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (comment != null)
                {
                    comment.Id = id;
                    comment.DriveId = vm.DriveId;
                    comment.CommentText = vm.CommentText;
                    comment.UpdatedBy = User.Identity!.Name;
                    comment.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _uow.Comments.Update(comment);
                }

                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (comment != null && !CommentExists(comment.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: AdminArea/Comments/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteCommentViewModel();
        if (id == null) return NotFound();

        var roleName = User.GettingUserRoleName();
        var comment = await _uow.Comments.GettingTheFirstCommentAsync(id.Value, null, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
#warning Ask maybe can be done as a base method

        vm.Drive = _uow.Comments.PickUpDateAndTimeStr(comment);
        vm.CustomerName = comment.Drive!.Booking!.Customer!.AppUser!.LastAndFirstName;
        vm.DriverName = comment.Drive!.Booking!.Driver!.AppUser!.LastAndFirstName;
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;
        vm.CreatedAt = comment.CreatedAt.ToLocalTime().ToString("G");
        vm.CreatedBy = comment.CreatedBy!;
        vm.UpdatedAt = comment.UpdatedAt.ToLocalTime().ToString("G");
        vm.UpdatedBy = comment.UpdatedBy!;

        return View(vm);
    }

    // POST: AdminArea/Comments/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var roleName = User.GettingUserRoleName();
        var comment = await _uow.Comments.GettingTheFirstCommentAsync(id, null, roleName);
        if (comment != null) _uow.Comments.Remove(comment);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CommentExists(Guid id)
    {
        return _uow.Comments.Exists(id);
    }
}