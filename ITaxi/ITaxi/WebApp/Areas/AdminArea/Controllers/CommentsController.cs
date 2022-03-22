#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;
using WebApp.Areas.AdminArea.ViewModels;

namespace WebApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CommentsController : Controller
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/Comments
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Comments
                .Include(c => c.Drive)
                .ThenInclude(d => d.Booking);
            return View(await appDbContext.ToListAsync());
        }

        // GET: AdminArea/Comments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeleteCommentViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Drive)
                .Include(c => c.Drive)
                .ThenInclude(d => d.Booking)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            vm.Id = comment.Id;
            vm.Drive = comment.Drive!.Booking!.PickUpDateAndTime.ToString("g");
            if (comment.CommentText != null) vm.CommentText = comment.CommentText;

            return View(vm);
        }

        // GET: AdminArea/Comments/Create
        public async Task<IActionResult> Create()
        {
            var vm = new CreateEditCommentViewModel();
            vm.Drives = await _context.Drives.Include(d => d.Booking)
                .Where(d => d.Comment.DriveId == null)
                .Select(d => new SelectListItem(d.Booking.PickUpDateAndTime.ToString("g"),
                    d.Id.ToString())).ToListAsync();
            return View(vm);
        }

        // POST: AdminArea/Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditCommentViewModel vm)
        {
            
            if (ModelState.IsValid)
            {
                var comment = new Comment()
                {
                    Id = Guid.NewGuid(),
                    DriveId = vm.DriveId,
                    CommentText = vm.CommentText
                };
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
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

            var comment = await _context.Comments.Include(c => c.Drive)
                .ThenInclude(c => c.Booking)
                .SingleOrDefaultAsync(c => c.Id.Equals(id));
            if (comment == null)
            {
                return NotFound();
            }

            vm.Drives = await _context.Drives.Include(d => d.Booking)
                .Select(d => new SelectListItem(d.Booking.PickUpDateAndTime.ToString("g"), d.Id.ToString()))
                .ToListAsync();
            if (comment.CommentText != null) vm.CommentText = comment.CommentText;
            vm.DriveId = comment.DriveId;

            return View(vm);
        }

        // POST: AdminArea/Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditCommentViewModel vm)
        {
            var comment = await _context.Comments.Include(c => c.Drive)
                .ThenInclude(c => c.Booking).SingleOrDefaultAsync(c => c.Id.Equals(id));
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
                        _context.Update(comment);
                    }

                    await _context.SaveChangesAsync();
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

            var comment = await _context.Comments
                .Include(c => c.Drive)
                .Include(c => c.Drive)
                .ThenInclude(c => c.Booking)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            vm.Id = comment.Id;
            vm.Drive = comment.Drive!.Booking!.PickUpDateAndTime.ToString("g");
            if (comment.CommentText != null) vm.CommentText = comment.CommentText;

            return View(vm);
        }

        // POST: AdminArea/Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var comment = await _context.Comments.SingleOrDefaultAsync(c => c.Id.Equals(id));
            if (comment != null) _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(Guid id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
