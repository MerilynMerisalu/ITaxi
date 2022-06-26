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
    public class PhotosController : Controller
    {
        private readonly AppDbContext _context;

        public PhotosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdminArea/Photos
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Photos.Include(p => p.AppUser);
            return View(await appDbContext.ToListAsync());
        }

        // GET: AdminArea/Photos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            var vm = new DetailsDeletePhotoViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            photo.Id = vm.Id;
            photo.Title = vm.Title;
            photo.PhotoName = vm.PhotoName;
            return View(vm);
        }

        // GET: AdminArea/Photos/Create
        public IActionResult Create()
        {
            var vm = new CreateEditPhotoViewModel();
            //ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Email");
            return View(vm);
        }

        // POST: AdminArea/Photos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEditPhotoViewModel vm, Photo photo)
        {
            if (ModelState.IsValid)
            {
                photo.Id = Guid.NewGuid();
                photo.Title = vm.Title;
                photo.PhotoName = vm.PhotoName;
                _context.Add(photo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Email", photo.AppUserId);
            return View(vm);
        }

        // GET: AdminArea/Photos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            var vm = new CreateEditPhotoViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos.FindAsync(id);
            if (photo == null)
            {
                return NotFound();
            }
            //ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Email", photo.AppUserId);
            return View(vm);
        }

        // POST: AdminArea/Photos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateEditPhotoViewModel vm)
        {
            var photo =  await _context.Photos.FindAsync(id);
            if (photo != null && id != photo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photo!);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (photo != null && !PhotoExists(photo.Id))
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
            //ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Email", photo.AppUserId);
            return View(vm);
        }

        // GET: AdminArea/Photos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            var vm = new DetailsDeletePhotoViewModel();
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photos
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photo == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // POST: AdminArea/Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo != null)
            {
                _context.Photos.Remove(photo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoExists(Guid id)
        {
          return (_context.Photos.Any(e => e.Id == id));
        }
    }
}
