#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.DriverArea;

[Route("api/DriverArea/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, Driver", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PhotosController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;

    public PhotosController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: api/Photos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Photo>>> GetPhotos()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _uow.Photos.GetAllPhotosWithIncludesAsync(userId, roleName);
        foreach (var photo in res)
        {
            if (photo != null)
            {
                photo.CreatedAt = photo.CreatedAt.ToLocalTime();
                photo.UpdatedAt = photo.UpdatedAt.ToLocalTime();
            }
        }
        return Ok(res);
    }

    // GET: api/Photos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Photo>> GetPhoto(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var photo = await _uow.Photos.GetPhotoByIdAsync(id, userId, roleName);
        if (photo == null) return NotFound();
        photo.CreatedAt = photo.CreatedAt.ToLocalTime();
        photo.UpdatedAt = photo.UpdatedAt.ToLocalTime();

        return photo;
    }

    // PUT: api/Photos/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPhoto(Guid id, Photo? photo)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
         photo = await _uow.Photos.GetPhotoByIdAsync(id, userId, roleName);
        if (photo == null) return NotFound();
        
        try
        {
            photo.UpdatedBy = User.Identity!.Name;
            photo.UpdatedAt = DateTime.Now.ToUniversalTime();
            _uow.Photos.Update(photo);
            await _uow.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PhotoExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Photos
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Photo>> PostPhoto(Photo photo)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        if (roleName != nameof(Admin) || photo.AppUserId != userId)
        {
            return Forbid();
        }

        photo.AppUserId = userId;
        photo.CreatedAt = photo.CreatedAt.ToLocalTime();
        photo.UpdatedAt = photo.UpdatedAt.ToLocalTime();

        _uow.Photos.Add(photo);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetPhoto", new {id = photo.Id}, photo);
    }

    // DELETE: api/Photos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePhoto(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var photo = await _uow.Photos.GetPhotoByIdAsync(id, userId, roleName);
        if (photo == null) return NotFound();

        _uow.Photos.Remove(photo);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool PhotoExists(Guid id)
    {
        return (_uow.Photos?.Exists(id)).GetValueOrDefault();
    }
}