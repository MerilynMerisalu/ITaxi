#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Public.DTO.v1.AdminArea;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

/// <summary>
/// Api controller for photos
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PhotosController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for photos api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.PhotoDTO to Public.DTO.v1.AdminArea.Photo</param>
    public PhotosController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/Photos
    /// <summary>
    /// Gets all the photos
    /// </summary>
    /// <returns>List of photos with a statusCode 200OK or statusCode 403 or statusCode 401</returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<Photo>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<Photo>>> GetPhotos()
    {
        var res = await _appBLL.Photos.GetAllPhotosWithIncludesAsync();
        return Ok(res.Select(p=> _mapper.Map<Photo>(p)));
    }

    // GET: api/Photos/5
    /// <summary>
    /// Returns photo based on id
    /// </summary>
    /// <param name="id">Photo id, Guid</param>
    /// <returns>Photo (TEntity) with statusCode 200 or statusCode 404 or statusCode 403 or statusCode 401</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Photo), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Photo>> GetPhoto(Guid id)
    {
        var photo = await _appBLL.Photos.FirstOrDefaultAsync(id);

        if (photo == null) return NotFound();

        return _mapper.Map<Photo>(photo);
    }

    // PUT: api/Photos/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating a photo
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="photo">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400</returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutPhoto(Guid id, Photo photo)
    {
        if (id != photo.Id) return BadRequest();

        var photoDTO = await _appBLL.Photos.FirstOrDefaultAsync(id);

        try
        {
            if (photoDTO != null)
            {
                photoDTO.UpdatedBy = User.GettingUserEmail();
                photoDTO.UpdatedAt = DateTime.Now.ToUniversalTime();
                _appBLL.Photos.Update(photoDTO);
            }
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
    /// <summary>
    /// Creating a new photo
    /// </summary>
    /// <param name="photo">Photo with properties</param>
    /// <returns>Status201Created with an entity</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Photo), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Photo>> PostPhoto([FromBody]Photo photo)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }

        var photoDTO = new PhotoDTO();
        photoDTO.Id = Guid.NewGuid();
        photoDTO.CreatedBy = User.GettingUserEmail();
        photoDTO.CreatedAt = DateTime.Now.ToUniversalTime();
        photoDTO.UpdatedBy = User.GettingUserEmail();
        photoDTO.UpdatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.Photos.Add(photoDTO);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetPhoto", new {id = photo.Id, 
            version = HttpContext.GetRequestedApiVersion()!.ToString()}, photo);
    }

    // DELETE: api/Photos/5
    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="id">Id of an entity</param>
    /// <returns>Status204</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeletePhoto(Guid id)
    {
        var photo = await _appBLL.Photos.FirstOrDefaultAsync(id);
        if (photo == null) return NotFound();

        _appBLL.Photos.Remove(photo);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }
    
    /// <summary>
    /// Return a boolean based on whether or not an entity exists
    /// </summary>
    /// <param name="id">Entity id guid</param>
    /// <returns>boolean value</returns>

    private bool PhotoExists(Guid id)
    {
        return (_appBLL.Photos.Exists(id));
    }
}