#nullable disable
using App.Contracts.DAL;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[Route("api/AdminArea/[controller]")]
[ApiController]
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
        return Ok(await _uow.Photos.GetAllAsync());
    }

    // GET: api/Photos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Photo>> GetPhoto(Guid id)
    {
        var photo = await _uow.Photos.FirstOrDefaultAsync(id);

        if (photo == null) return NotFound();

        return photo;
    }

    // PUT: api/Photos/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPhoto(Guid id, Photo photo)
    {
        if (id != photo.Id) return BadRequest();


        try
        {
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
        _uow.Photos.Add(photo);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetPhoto", new {id = photo.Id}, photo);
    }

    // DELETE: api/Photos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePhoto(Guid id)
    {
        var photo = await _uow.Photos.FirstOrDefaultAsync(id);
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