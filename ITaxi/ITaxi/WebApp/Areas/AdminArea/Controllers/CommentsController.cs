#nullable enable
using App.Contracts.DAL;
using App.DAL.EF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area(nameof(AdminArea))]
    public class CommentsController : Controller
    {
        private readonly IAppUnitOfWork _uow;
        private readonly AppDbContext _context;

        public CommentsController(IAppUnitOfWork uow, AppDbContext context)
        {
            _uow = uow;
            _context = context;
        }

        // GET: AdminArea/Comments
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Comments.GettingAllOrderedCommentsWithIncludesAsync());
        }

        // GET: AdminArea/Comments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteCommentViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _uow.Comments.FirstOrDefaultAsync(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            vm.Id = comment.Id;
            vm.Drive = _uow.Comments.PickUpDateAndTimeStr(comment);
            if (comment.CommentText != null) vm.CommentText = comment.CommentText;

            return View(vm);
        }

        // GET: AdminArea/Comments/Create
        public async Task<IActionResult> Create()
        {
            var vm = new CreateEditCommentViewModel();
            // vm.Drives = new SelectList( await _context.Drives.Include(d => d.Booking)
            //         .Where(d => d.Comment.DriveId == null)
            //         .Select(d => new {d.Booking.PickUpDateAndTime, d.Id}).ToListAsync()
            vm.Drives = new SelectList(await _uow.Drives.GettingDrivesWithoutCommentAsync(),
                nameof(Drive.Id), nameof(Drive.Booking.PickUpDateAndTime));
            
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
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _uow.Comments.FirstOrDefaultAsync(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            vm.Id = comment.Id;

            vm.Drives = new SelectList(await _uow.Drives.GettingAllOrderedDrivesWithIncludesAsync(), nameof(Drive.Id),
                nameof(Drive.Booking.PickUpDateAndTime),
                nameof(vm.DriveId));
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
            if (comment != null && id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (comment != null)
                    {
                        comment.Id = id;
                        comment.DriveId = vm.DriveId;
                        comment.CommentText = vm.CommentText;
                        _uow.Comments.Update(comment);
                    }

                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (comment != null && !CommentExists(comment.Id))
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

        // GET: AdminArea/Comments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var vm = new DetailsDeleteCommentViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _uow.Comments.FirstOrDefaultAsync(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            vm.Id = comment.Id;
            #warning Ask maybe can be done as a base method

            vm.Drive = _uow.Comments.PickUpDateAndTimeStr(comment);
            if (comment.CommentText != null) vm.CommentText = comment.CommentText;

            return View(vm);
        }

        // POST: AdminArea/Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var comment = await _uow.Comments.FirstOrDefaultAsync(id);
            if (comment != null) _uow.Comments.Remove(comment);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(Guid id)
        {
            return _uow.Comments.Exists(id);
        }
    }
}
