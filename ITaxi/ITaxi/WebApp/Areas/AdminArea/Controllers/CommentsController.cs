#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Drive)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
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
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["DriveId"] = new SelectList(_context.Drives, "Id", "Id", comment.DriveId);
            return View(comment);
        }

        // POST: AdminArea/Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DriveId,CommentText,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            ViewData["DriveId"] = new SelectList(_context.Drives, "Id", "Id", comment.DriveId);
            return View(comment);
        }

        // GET: AdminArea/Comments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Drive)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: AdminArea/Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(Guid id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
