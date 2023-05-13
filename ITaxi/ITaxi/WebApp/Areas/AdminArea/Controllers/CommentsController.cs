#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.AdminArea.ViewModels;


namespace WebApp.Areas.AdminArea.Controllers;

[Authorize(Roles = "Admin")]
[Area(nameof(AdminArea))]
public class CommentsController : Controller
{
    private readonly IAppBLL _appBLL;

    public CommentsController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/Comments
    public async Task<IActionResult> Index()
    {
        
#warning Ask how to get the user role using interface
        
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Comments.GettingAllOrderedCommentsWithIncludesAsync(roleName:roleName);
        return View(res);
    }

    // GET: AdminArea/Comments/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteCommentViewModel();
        if (id == null) return NotFound();

        var roleName = User.GettingUserRoleName();

        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id.Value, null, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.Drive = comment.Drive!.Booking!.PickUpDateAndTime.ToString("g");
        vm.CustomerName = comment.Drive!.Booking!.Customer!.AppUser!.LastAndFirstName;
        vm.DriverName = comment.Drive!.Booking!.Drive!.Driver!.AppUser!.LastAndFirstName;
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;
        vm.CreatedAt = comment.CreatedAt;
        vm.CreatedBy = comment.CreatedBy!;
        vm.UpdatedAt = comment.UpdatedAt;
        vm.UpdatedBy = comment.UpdatedBy!;


        return View(vm);
    }

    // GET: AdminArea/Comments/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateCommentViewModel();

        var roleName = User.GettingUserRoleName();
        var drives = await _appBLL.Drives.GettingDrivesWithoutCommentAsync(null, roleName);
        /*foreach (var drive in drives)
        {
            if (drive != null) drive.Booking!.PickUpDateAndTime = drive.Booking.PickUpDateAndTime.ToLocalTime();
        }*/
        vm.Drives = new SelectList(drives,
            nameof(App.BLL.DTO.AdminArea.DriveDTO.Id), nameof(App.BLL.DTO.AdminArea.DriveDTO.DriveDescription));

        return View(vm);
    }

    // POST: AdminArea/Comments/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCommentViewModel vm)
    {
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

        vm.Drives = new SelectList(await _appBLL.Drives.GettingDrivesWithoutCommentAsync(null, roleName),
            nameof(DriverDTO.Id),
            nameof(BookingDTO.DriveTime));


        return View(vm);
    }

    // GET: AdminArea/Comments/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var roleName = User.GettingUserRoleName();
        var vm = new EditCommentViewModel();
        if (id == null) return NotFound();

        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id.Value, null, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.Id = comment.Id;
        comment.Drive!.Booking!.PickUpDateAndTime = comment.Drive.Booking.PickUpDateAndTime.ToLocalTime();
        
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;
        vm.DriveId = comment.Drive!.Id;
        vm.DriveTimeAndDriver = comment.Drive.DriveDescription;

        return View(vm);
    }

    // POST: AdminArea/Comments/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditCommentViewModel vm)
    {
        var roleName = User.GettingUserRoleName();
        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id, null, roleName);
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

        var roleName = User.GettingUserRoleName();
        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id.Value, null, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.Drive = comment.Drive!.Booking!.PickUpDateAndTime.ToString("g");
        vm.CustomerName = comment.Drive!.Booking!.Customer!.AppUser!.LastAndFirstName;
        vm.DriverName = comment.Drive.Driver!.AppUser!.LastAndFirstName;
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;
        vm.CreatedAt = comment.CreatedAt;
        vm.CreatedBy = comment.CreatedBy!;
        vm.UpdatedAt = comment.UpdatedAt;
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
        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id, null, roleName, noIncludes:true);
        if (comment != null) _appBLL.Comments.Remove(comment);
        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CommentExists(Guid id)
    {
        return _appBLL.Comments.Exists(id);
    }
}