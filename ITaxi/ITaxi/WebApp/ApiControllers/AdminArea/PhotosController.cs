#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        return Ok(await _appBLL.Photos.GetAllPhotosWithIncludesAsync());
    }

    // GET: api/Photos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PhotoDTO>> GetPhoto(Guid id)
    {
        var photo = await _appBLL.Photos.FirstOrDefaultAsync(id);

        if (photo == null) return NotFound();

        return photo;
    }

    // PUT: api/Photos/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPhoto(Guid id, PhotoDTO photo)
    {
        if (id != photo.Id) return BadRequest();


        try
        {
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
    public async Task<ActionResult<PhotoDTO>> PostPhoto([FromBody]PhotoDTO photo)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }
        _appBLL.Photos.Add(photo);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetPhoto", new {id = photo.Id, 
            version = HttpContext.GetRequestedApiVersion()!.ToString()}, photo);
    }

    // DELETE: api/Photos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePhoto(Guid id)
    {
        var photo = await _appBLL.Photos.FirstOrDefaultAsync(id);
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