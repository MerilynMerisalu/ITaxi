#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
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
    private readonly IAppBLL _appBLL;

    public PhotosController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: api/Photos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PhotoDTO>>> GetPhotos()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Photos.GetAllPhotosWithIncludesAsync(userId, roleName);
        
        return Ok(res);
    }

    // GET: api/Photos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PhotoDTO>> GetPhoto(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var photo = await _appBLL.Photos.GetPhotoByIdAsync(id, userId, roleName);
        if (photo == null) return NotFound();
        photo.CreatedAt = photo.CreatedAt;
        photo.UpdatedAt = photo.UpdatedAt;

        return photo;
    }

    // PUT: api/Photos/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPhoto(Guid id, PhotoDTO? photo)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
         photo = await _appBLL.Photos.GetPhotoByIdAsync(id, userId, roleName);
        if (photo == null) return NotFound();
        
        try
        {
            photo.UpdatedBy = User.Identity!.Name;
            photo.UpdatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.Photos.Update(photo);
            await _appBLL.SaveChangesAsync();
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
    public async Task<ActionResult<PhotoDTO>> PostPhoto(PhotoDTO photo)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        if (roleName != "Admin" || photo.AppUserId != userId)
        {
            return Forbid();
        }

        photo.AppUserId = userId;
        photo.CreatedAt = photo.CreatedAt;
        photo.UpdatedAt = photo.UpdatedAt;

        _appBLL.Photos.Add(photo);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetPhoto", new {id = photo.Id}, photo);
    }

    // DELETE: api/Photos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePhoto(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var photo = await _appBLL.Photos.GetPhotoByIdAsync(id, userId, roleName);
        if (photo == null) return NotFound();

        _appBLL.Photos.Remove(photo);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool PhotoExists(Guid id)
    {
        return (_appBLL.Photos?.Exists(id)).GetValueOrDefault();
    }
}