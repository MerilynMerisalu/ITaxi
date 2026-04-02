#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.CustomerArea.ViewModels;

namespace WebApp.Areas.CustomerArea.Controllers;

/// <summary>
/// Customer area comments controller
/// </summary>
[Authorize(Roles = "Admin, Customer")]
[Area(nameof(CustomerArea))]
public class CommentsController : Controller
{
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Customer area comments controller constructor
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public CommentsController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: CustomerArea/Comments
    /// <summary>
    /// Customer area comments index
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Index()
    {
        var userId = User.GetUserId();
        var roleName = User.GetUserRoleName();
        var res = await _appBLL.Comments.
            GettingAllOrderedCommentsWithIncludesAsync(userId, roleName);
        
        return View(res);
    }

    // GET: CustomerArea/Comments/Details/5
    /// <summary>
    /// Customer area comments GET method details
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Details(Guid? id)
    {
        var userId = User.GetUserId();
        var roleName = User.GetUserRoleName();
        var vm = new DetailsDeleteCommentViewModel();
        if (id == null) return NotFound();

        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id.Value, userId, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.Drive = comment.DriveCustomerStr;
        vm.DriverName = comment.DriverName;
        vm.BookingNumber = comment.BookingNumber;
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;
        if (comment.StarRating > 0)
        {
            vm.StarRating = comment.StarRating.Value;
        }
        
        return View(vm);
    }
/// <summary>
/// Get booking number
/// </summary>
/// <param name="id">Drive id</param>
/// <returns>Booking number</returns>
    [HttpPost("/CustomerArea/Comments/GetBookingNumber")]
    public async Task<IActionResult> GetBookingNumber([FromBody]Guid? id = null)
    {
        if (id == null) return NotFound();
        var bookingNumber = await _appBLL.Bookings.GettingBookingNumberByDriveIdAsync(id.Value, 
            User.GetUserId());
        return Content(bookingNumber.ToString() ?? String.Empty, contentType: "text/plain");
        
    }
    
    // GET: AdminArea/Comments/Create
    /// <summary>
    /// Customer area comments GET method create
    /// </summary>
    /// <returns>View</returns>
    public async Task<IActionResult> Create()
    {
        var vm = new CreateCommentViewModel();
        var userId = User.GetUserId();
        var roleName = User.GetUserRoleName();

        var drives = await _appBLL.Drives.GettingFinishedDrivesWithoutCommentAsync(userId, roleName);
        foreach (var drive in drives)
        {
            if (drive != null && drive.Booking != null)
                drive.Booking.PickUpDateAndTime = drive.Booking.PickUpDateAndTime.ToLocalTime();
        }
        vm.Drives = new SelectList(drives,
            nameof(Drive.Id), nameof(Drive.DriveDescription));

        return View(vm);
    }

    // POST: AdminArea/Comments/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Customer area comments POST method create
    /// </summary>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCommentViewModel vm)
    {var userId = User.GetUserId();
        var roleName = User.GetUserRoleName();
        var comment = new CommentDTO();
        if (ModelState.IsValid)
        {
            comment.Id = Guid.NewGuid();
            comment.DriveId = vm.DriveId;
            comment.CommentText = vm.CommentText;
            if (vm.StarRating != null && vm.StarRating.Value > 0)
            {
                comment.StarRating = vm.StarRating.Value;
            }
            comment.CreatedBy = User.Identity!.Name;
            comment.CreatedAt = DateTime.Now.ToUniversalTime();

            _appBLL.Comments.Add(comment);
            await _appBLL.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.Drives = new SelectList(await _appBLL.Drives.GettingFinishedDrivesWithoutCommentAsync(userId, roleName),
            nameof(Drive.Id),
            nameof(Drive.DriveDescription));
        
        return View(vm);
    }

    // GET: AdminArea/Comments/Edit/5
    /// <summary>
    /// Customer area comments GET method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Edit(Guid? id)
    {
        var userId = User.GetUserId();
        var roleName = User.GetUserName();
        var vm = new EditCommentViewModel();
        if (id == null) return NotFound();
        
        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id.Value, userId, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.BookingNumber = comment.BookingNumber;
        vm.DriveTimeAndDriver = $"{comment.DriveCustomerStr} - {comment.DriverName}";
        if (comment.StarRating != null) vm.StarRating = comment.StarRating.Value;
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;
        vm.DriveId = comment.DriveId;
        
        return View(vm);
    }

    // POST: AdminArea/Comments/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Customer area comments POST method edit
    /// </summary>
    /// <param name="id">Id</param>
    /// <param name="vm">View model</param>
    /// <returns>View</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditCommentViewModel vm)
    {
        var userId = User.GetUserId();
        var roleName = User.GetUserRoleName();
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
                    if (comment.StarRating != null
                        && vm.StarRating != null
                        && vm.StarRating.Value != comment.StarRating.Value
                        && vm.StarRating.Value > 0)
                    {
                            comment.StarRating = vm.StarRating!.Value;
                    }
                    comment.UpdatedBy = User.Identity!.Name;
                    comment.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _appBLL.Comments.Update(comment);
                

                    await _appBLL.SaveChangesAsync();
                }

                
                
                    
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
    /// Customer area comments GET method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>View</returns>
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteCommentViewModel();
        if (id == null) return NotFound();
        var userId = User.GetUserId();
        var roleName = User.GetUserName();
        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id.Value, userId, roleName);
        if (comment == null) return NotFound();

        vm.Id = comment.Id;
        vm.Drive = comment.DriveCustomerStr;

        vm.DriverName = comment.DriverName;
        if (comment.CommentText != null) vm.CommentText = comment.CommentText;
        if (comment.StarRating > 0)
            vm.StarRating = comment.StarRating.Value;
        
        return View(vm);
    }

    // POST: AdminArea/Comments/Delete/5
    /// <summary>
    /// Customer area comments POST method delete
    /// </summary>
    /// <param name="id">Id</param>
    /// <returns>Redirect to index</returns>
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var userId = User.GetUserId();
        var roleName = User.GetUserName();
        var comment = await _appBLL.Comments.GettingTheFirstCommentAsync(id, userId, roleName, noIncludes:true);
        if (comment != null)
        {
            comment.DriveId = null;
            _appBLL.Comments.Update(comment);
            await _appBLL.SaveChangesAsync();
            await _appBLL.Comments.RemoveAsync(comment.Id);
        }
        await _appBLL.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CommentExists(Guid id)
    {
        return _appBLL.Comments.Exists(id);
    }
}