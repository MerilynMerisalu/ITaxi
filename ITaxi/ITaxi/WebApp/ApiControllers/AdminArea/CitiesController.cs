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

public class CitiesController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Constructor for cities api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.CitiesDTO to 
    /// Public.DTO.v1.AdminArea.City</param>
    public CitiesController(IAppBLL appBLL, IMapper mapper)
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
    [ProducesResponseType( typeof( IEnumerable<City>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<City>>> GetCities()
    {
        var cities = await _appBLL.Cities.GetAllAsync();
        return Ok(cities.Select(c => _mapper.Map<City>(c)));
    }

    // GET: api/Cities/5
    /// <summary>
    /// Returns city based on id
    /// </summary>
    /// <param name="id">City id, Guid</param>
    /// <returns>City (TEntity) with statusCode 200 or status404 or Status403 or Status401</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(City), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<City>> GetCity(Guid id)
    {
        var city = await _appBLL.Cities.FirstOrDefaultAsync(id);

        if (city == null) return NotFound();

        return _mapper.Map<City>(city);
    }

    // PUT: api/Cities/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating a city
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="city">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400</returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutCity(Guid id, City city)
    {
        if (id != city.Id) return BadRequest();
        var cityDTO = await _appBLL.Cities.FirstOrDefaultAsync(id);
        if (cityDTO == null)
        {
            return NotFound();
        }
        try
        {
            cityDTO.CityName = city.CityName;
            cityDTO.CountyId = city.CountyId;
            cityDTO.CreatedBy = User.GettingUserEmail();
            cityDTO.UpdatedBy = User.GettingUserEmail();
            _appBLL.Cities.Update(cityDTO);
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
    /// Creating a new city
    /// </summary>
    /// <param name="city">CityDTO with properties</param>
    /// <returns>Status201Created with an entity</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(City), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<City>> PostCity([FromBody] City city)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }

        var dto = _mapper.Map<CityDTO>(city);
        
        dto.CreatedBy = User.GettingUserEmail();
        _appBLL.Cities.Add(dto);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetCity", new
            {
                id = city.Id,
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
        var city = await _appBLL.Cities.FirstOrDefaultAsync(id);
        if (city == null) return NotFound();

        _appBLL.Cities.Remove(city);
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
        return _appBLL.Cities.Exists(id);
    }
}