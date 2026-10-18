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
/// Api controller for cities
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class MunicipalitiesController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Constructor for cities api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.CitiesDTO to 
    /// Public.DTO.v1.AdminArea.City</param>
    public MunicipalitiesController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/Cities
    /// <summary>
    /// Gets all the cities 
    /// </summary>
    /// <returns>List of cities with statusCode 200 or statusCode 403 or statusCode 401</returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<Municipality>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Municipality>>> GetCities()
    {
        var cities = await _appBLL.Municipalities.GetAllAsync();
        return Ok(cities.Select(c => _mapper.Map<Municipality>(c)));
    }

    // GET: api/Cities/5
    /// <summary>
    /// Returns muncipality based on id
    /// </summary>
    /// <param name="id">City id, Guid</param>
    /// <returns>City (TEntity) with statusCode 200 or status404 or Status403 or Status401</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Municipality), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Municipality>> GetCity(Guid id)
    {
        var muncipality = await _appBLL.Municipalities.FirstOrDefaultAsync(id);

        if (muncipality == null) return NotFound();

        return _mapper.Map<Municipality>(muncipality);
    }

    // PUT: api/Cities/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating a muncipality
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="municipality">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400</returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutCity(Guid id, Municipality municipality)
    {
        if (id != municipality.Id) return BadRequest();
        var municipalityDTO = await _appBLL.Municipalities.FirstOrDefaultAsync(id);
        if (municipalityDTO == null)
        {
            return NotFound();
        }
        try
        {
            municipalityDTO.Name = municipality.Name;
            municipalityDTO.CountyId = municipality.CountyId;
            municipalityDTO.CreatedBy = User.GetUserEmail();
            municipalityDTO.UpdatedBy = User.GetUserEmail();
            _appBLL.Municipalities.Update(municipalityDTO);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CityExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Cities
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Creating a new muncipality
    /// </summary>
    /// <param name="municipality">CityDTO with properties</param>
    /// <returns>Status201Created with an entity</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(Municipality), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Municipality>> PostCity([FromBody] Municipality municipality)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }

        var dto = _mapper.Map<MunicipalityDTO>(municipality);
        
        dto.CreatedBy = User.GetUserEmail();
        _appBLL.Municipalities.Add(dto);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetCity", new
            {
                id = municipality.Id,
                version = HttpContext.GetRequestedApiVersion()!.ToString() ,
            },
            dto);
    }

    // DELETE: api/Cities/5
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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    
    public async Task<IActionResult> DeleteCity(Guid id)
    {
        var city = await _appBLL.Municipalities.FirstOrDefaultAsync(id);
        if (city == null) return NotFound();

        await _appBLL.Municipalities.RemoveAsync(city.Id);
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
    private bool CityExists(Guid id)
    {
        return _appBLL.Municipalities.Exists(id);
    }
}