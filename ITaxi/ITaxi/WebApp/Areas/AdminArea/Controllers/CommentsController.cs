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

/// <summary>
/// Admin area comments controller
/// </summary>
[Authorize(Roles = "Admin")]
[Area(nameof(AdminArea))]
public class CommentsController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Admin area comments controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public CommentsController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: AdminArea/Comments
    /// <summary>
    /// Admin area comments index
    /// </summary>
    /// <returns>Index page view with data</returns>
    public async Task<IActionResult> Index()
    {
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Comments.GettingAllOrderedCommentsWithIncludesAsync(roleName:roleName);
        return View(res);
    }

    // GET: AdminArea/Comments/Details/5
    /// <summary>
    /// Admin area comment details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View model</returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteCommentViewModel();
        if (id == null) return NotFound();

        var roleName = User.GettingUserRoleName();

        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id.Value, null, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.Drive = comment.DriveCustomerStr;
        vm.CustomerName = comment.CustomerName;
        vm.DriverName = comment.DriverName;
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;
        if (comment.StarRating != null) vm.StarRating = comment.StarRating;
        vm.CreatedAt = comment.CreatedAt;
        vm.CreatedBy = comment.CreatedBy!;
        vm.UpdatedAt = comment.UpdatedAt;
        vm.UpdatedBy = comment.UpdatedBy!;
        
        return View(vm);
    }

    // GET: AdminArea/Comments/Create
    /// <summary>
    /// Admin area comment create
    /// </summary>
    /// <returns>View model</returns>
    public async Task<IActionResult> Create()
    {
        var vm = new CreateCommentViewModel();

        var roleName = User.GettingUserRoleName();
        var drives = await _appBLL.Drives.GettingDrivesWithoutCommentAsync(null, roleName);
        
        vm.Drives = new SelectList(drives,
            nameof(DriveDTO.Id), nameof(DriveDTO.DriveDescription));

        return View(vm);
    }

    // POST: AdminArea/Comments/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area comment create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <returns>View model</returns>
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

            if (vm.StarRating > 0)
            {
                comment.StarRating = vm.StarRating;
                
            }
            
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
    /// <summary>
    /// Admin area comment edit GET method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View model</returns>
    public async Task<IActionResult> Edit(Guid? id)
    {
        var roleName = User.GettingUserRoleName();
        var vm = new EditCommentViewModel();
        if (id == null) return NotFound();

        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id.Value, null, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.Id = comment.Id;
        if (comment.StarRating != null) vm.StarRating = comment.StarRating;
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;
        vm.DriveId = comment.DriveId;
        vm.DriveTimeAndDriver = $"{comment.DriveCustomerStr} - {comment.DriverName}";

        return View(vm);
    }

    // POST: AdminArea/Comments/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Admin area comment edit POST method
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View model</returns>
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
                    comment.StarRating = vm.StarRating;
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
    /// <summary>
    /// Admin area comment delete GET method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View model</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteCommentViewModel();
        if (id == null) return NotFound();

        var roleName = User.GettingUserRoleName();
        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id.Value, null, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.Drive = comment.DriveCustomerStr;
        vm.CustomerName = comment.CustomerName;
        vm.DriverName = comment.DriverName;
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;
        if (comment.StarRating != null) vm.StarRating = comment.StarRating;
        vm.CreatedAt = comment.CreatedAt;
        vm.CreatedBy = comment.CreatedBy!;
        vm.UpdatedAt = comment.UpdatedAt;
        vm.UpdatedBy = comment.UpdatedBy!;

        return View(vm);
    }

    // POST: AdminArea/Comments/Delete/5
    /// <summary>
    /// Admin area comment delete POST method
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect user to Comment index page</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var roleName = User.GettingUserRoleName();
        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id, null, roleName, noIncludes:true);
        if (comment != null && comment.DriveId != null)
        {
            comment.DriveId = null;
            _appBLL.Comments.Update(comment);
            await _appBLL.SaveChangesAsync();
                   
        }
        if (comment != null) await _appBLL.Comments.RemoveAsync(comment.Id);
        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CommentExists(Guid id)
    {
        return _appBLL.Comments.Exists(id);
    }
}