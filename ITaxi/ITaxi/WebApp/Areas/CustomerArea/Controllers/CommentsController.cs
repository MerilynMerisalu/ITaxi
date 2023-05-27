#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
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
    private readonly IAppBLL _appBLL;

    public CommentsController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: CustomerArea/Comments
    public async Task<IActionResult> Index()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Comments.
            GettingAllOrderedCommentsWithIncludesAsync(userId, roleName);
        
        return View(res);
    }

    // GET: CustomerArea/Comments/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var vm = new DetailsDeleteCommentViewModel();
        if (id == null) return NotFound();

        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id.Value, userId, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.Drive = comment.DriveCustomerStr;
        vm.DriverName = comment.DriverName;
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;


        return View(vm);
    }

    // GET: AdminArea/Comments/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateCommentViewModel();
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();

        var drives = await _appBLL.Drives.GettingDrivesWithoutCommentAsync(userId, roleName);
        foreach (var drive in drives)
        {
            if (drive != null) drive.Booking!.PickUpDateAndTime = drive.Booking.PickUpDateAndTime.ToLocalTime();
        }
        vm.Drives = new SelectList(drives,
            nameof(Drive.Id), nameof(Drive.DriveDescription));

        return View(vm);
    }

    // POST: AdminArea/Comments/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCommentViewModel vm)
    {var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var comment = new CommentDTO();
        if (ModelState.IsValid)
        {
            comment.Id = Guid.NewGuid();
            comment.DriveId = vm.DriveId;
            comment.CommentText = vm.CommentText;
            comment.CreatedBy = User.Identity!.Name;
            comment.CreatedAt = DateTime.Now.ToUniversalTime();

            _appBLL.Comments.Add(comment);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.Drives = new SelectList(await _appBLL.Drives.GettingDrivesWithoutCommentAsync(userId, roleName),
            nameof(Drive.Id),
            nameof(Drive.DriveDescription));


        return View(vm);
    }

    // GET: AdminArea/Comments/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserName();
        var vm = new EditCommentViewModel();
        if (id == null) return NotFound();
        

        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id.Value, userId, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.DriveTimeAndDriver = $"{comment.DriveCustomerStr} - {comment.DriverName}";

        
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;
        vm.DriveId = comment.DriveId;


        return View(vm);
    }

    // POST: AdminArea/Comments/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditCommentViewModel vm)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id, userId, roleName, noIncludes:true);
        if (comment != null && id != comment.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (comment != null)
                {
                    comment.Id = id;
                    comment.CommentText = vm.CommentText;
                    comment.UpdatedBy = User.Identity!.Name;
                    comment.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _appBLL.Comments.Update(comment);
                }

                await _appBLL.SaveChangesAsync();
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
        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id.Value, userId, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.Drive = comment.DriveCustomerStr;

        vm.DriverName = comment.DriverName;
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
        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id, userId, roleName, noIncludes:true);
        if (comment != null) _appBLL.Comments.Remove(comment);
        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CommentExists(Guid id)
    {
        return _appBLL.Comments.Exists(id);
    }
}