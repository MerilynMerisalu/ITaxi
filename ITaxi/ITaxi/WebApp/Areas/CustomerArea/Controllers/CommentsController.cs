#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.CustomerArea.ViewModels;

namespace WebApp.Areas.CustomerArea.Controllers;

[Authorize(Roles = "Admin, Customer")]
[Area(nameof(CustomerArea))]
public class CommentsController : Controller
{
    private readonly IAppUnitOfWork _uow;

    public CommentsController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: CustomerArea/Comments
    public async Task<IActionResult> Index()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserName();
        var res = await _uow.Comments.GettingAllOrderedCommentsWithIncludesAsync(userId, roleName);
        foreach (var comment in res)
        {
            comment.Drive!.Booking!.PickUpDateAndTime = comment.Drive!.Booking.PickUpDateAndTime.ToLocalTime();
        }
        return View(res);
    }

    // GET: CustomerArea/Comments/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserName();
        var vm = new DetailsDeleteCommentViewModel();
        if (id == null) return NotFound();

        var comment = await _uow.Comments.GettingTheFirstCommentAsync(id.Value, userId, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.Drive = comment.Drive!.Booking!.PickUpDateAndTime.ToString("g");
        vm.DriverName = comment.Drive!.Booking!.Driver!.AppUser!.LastAndFirstName;
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;


        return View(vm);
    }

    // GET: AdminArea/Comments/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateEditCommentViewModel();
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();

        vm.Drives = new SelectList(await _uow.Drives.GettingDrivesWithoutCommentAsync(userId, roleName),
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

        vm.Drives = new SelectList(await _uow.Drives.GettingDrivesWithoutCommentAsync(),
            nameof(Drive.Id),
            nameof(Booking.DriveTime));


        return View(vm);
    }

    // GET: AdminArea/Comments/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new CreateEditCommentViewModel();
        if (id == null) return NotFound();

        var comment = await _uow.Comments.FirstOrDefaultAsync(id.Value);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;

        vm.Drives = new SelectList(await _uow.Drives.GettingAllDrivesForCommentsAsync(), nameof(Drive.Id),
            nameof(Drive.DriveDescription),
            nameof(Drive.Id));
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
        var comment = await _uow.Comments.FirstOrDefaultAsync(id);
        if (comment != null && id != comment.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (comment != null)
                {
                    comment.Id = id;
                    comment.CommentText = vm.CommentText;
                    comment.UpdatedAt = DateTime.Now;
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
        var userId = User.GettingUserId();
        var roleName = User.GettingUserName();
        var comment = await _uow.Comments.GettingTheFirstCommentAsync(id.Value, userId, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.Drive = comment.Drive!.Booking!.DriveTime;

        vm.DriverName = comment.Drive!.Booking!.Driver!.AppUser!.LastAndFirstName;
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;


        return View(vm);
    }

    // POST: AdminArea/Comments/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserName();
        var comment = await _uow.Comments.GettingTheFirstCommentAsync(id, userId, roleName);
        if (comment != null) _uow.Comments.Remove(comment);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CommentExists(Guid id)
    {
        return _uow.Comments.Exists(id);
    }
}