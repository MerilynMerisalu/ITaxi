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
/// Api controller for admins
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AdminsController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for admins api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.CitiesDTO to
    /// Public.DTO.v1.AdminArea.City</param>
    public AdminsController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/Admins
    /// <summary>
    /// Gets all the admins 
    /// </summary>
    /// <returns>List of cities with statusCode 200 or statusCode 403 or statusCode 401</returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<City>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
    {
        var res = await _appBLL.Admins.GetAllAdminsOrderedByLastNameAsync();
        
        return Ok(res.Select(a => _mapper.Map<Admin>(a)));
    }

    // GET: api/Admins/5
    /// <summary>
    /// Returns city based on id
    /// </summary>
    /// <param name="id">City id, Guid</param>
    /// <returns>Admin (TEntity) with statusCode 200 or status404 or Status403 or Status401</returns>
    
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Admin), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Admin>> GetAdmin(Guid id)
    {
        var admin = await _appBLL.Admins.FirstOrDefaultAsync(id);

        if (admin == null) return NotFound();
        
        
        return _mapper.Map<Admin>(admin);
    }

    // PUT: api/Admins/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating an city
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="adminDTO">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400 </returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAdmin(Guid id, AdminDTO adminDTO)
    {
        if (id != adminDTO.Id) return BadRequest();

        var admin = await _appBLL.Admins.FirstOrDefaultAsync(id);
        if (admin == null)
        {
            return NotFound();
        }
        try
        {
            admin.CityId = adminDTO.CityId;
            admin.Address = adminDTO.Address;
            admin.PersonalIdentifier = adminDTO.PersonalIdentifier;
            admin.CreatedBy = User.GettingUserEmail();
            admin.UpdatedBy = User.GettingUserEmail();
            _appBLL.Admins.Update(admin);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AdminExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Admins
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Creating an admin
    /// </summary>
    /// <param name="admin"></param>
    /// <returns></returns>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AdminDTO>> PostAdmin([FromBody]AdminDTO admin)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }

        admin.Id = new Guid();
        admin.CreatedBy = User.GettingUserEmail();
        admin.UpdatedBy = User.GettingUserEmail();
        _appBLL.Admins.Add(admin);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetAdmin", new
        {
            id = admin.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString(),
        }, admin);
    }

    // DELETE: api/Admins/5
    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="id">Id of an entity</param>
    /// <returns>Status204, StatusCode 404, StatusCode 403, StatusCode 401</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteAdmin(Guid id)
    {
        var admin = await _appBLL.Admins.FirstOrDefaultAsync(id);
        if (admin == null) return NotFound();

        _appBLL.Admins.Remove(admin);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }
    
    /// <summary>
    /// Return a boolean based on whether or not an entity exists
    /// </summary>
    /// <param name="id">Entity id guid</param>
    /// <returns>Boolean value</returns>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private bool AdminExists(Guid id)
    {
        return _appBLL.Admins.Exists(id);
    }
}