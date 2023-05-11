#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Public.DTO.v1.AdminArea;
using AutoMapper;
using Base.Contracts;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers.AdminArea;

/// <summary>
/// Api controller for counties
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

public class CountiesController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for counties api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea to Public.DTO.v1.AdminArea.County</param>
    public CountiesController(IAppBLL appBLL, IMapper mapper)//<App.Public.DTO.v1.AdminArea.County, App.BLL.DTO.AdminArea>)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/counties
    /// <summary>
    /// Gets all the counties 
    /// </summary>
    /// <returns>List of counties with a statusCode 200OK or statusCode 403 or statusCode 401 </returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<County>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<County>>> GetCounties()
    {
        var res = await _appBLL.Counties.GetAllCountiesOrderedByCountyNameAsync();
        return Ok(res.Select(x => _mapper.Map<County>(x)).ToList());
    }

    // GET: api/Counties/5
    /// <summary>
    /// Returns county based on id
    /// </summary>
    /// <param name="id">County id, Guid</param>
    /// <returns>County(TEntity) with statusCode 200 or statusCode 404 or statusCode 403 or statusCode 401   </returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(County), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<County>> GetCounty(Guid id)
    {
        var county = await _appBLL.Counties.FirstOrDefaultAsync(id);

        if (county == null) return NotFound();

        return _mapper.Map<County>(county);
    }

    // PUT: api/Counties/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating an county 
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="county">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400 </returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutCounty(Guid id, County county)
    {
        var countyDTO = await _appBLL.Counties.FirstOrDefaultAsync(id);
        if (countyDTO == null)
        {
            return NotFound();
        }
        
        countyDTO.CountyName = county.CountyName;
        
        countyDTO.UpdatedBy = User.Identity!.Name;
        countyDTO.UpdatedAt = DateTime.Now;
        _appBLL.Counties.Update(countyDTO);
        await _appBLL.SaveChangesAsync();
        return NoContent();
    }

    // POST: api/Counties
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Creating a new county
    /// </summary>
    /// <param name="county">County with properties</param>
    /// <returns>Status201Created with an entity</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(County), 
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    
    public async Task<ActionResult<County>> PostCounty([FromBody] County county)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }

        var dto = _mapper.Map<CountyDTO>(county);
        
        dto.Id = Guid.NewGuid();
        dto.CreatedBy = User.GettingUserEmail();
        dto.UpdatedBy = User.GettingUserEmail();
        dto.CreatedAt = DateTime.Now.ToUniversalTime();
        dto.UpdatedAt = DateTime.Now.ToUniversalTime();

        _appBLL.Counties.Add(dto);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetCounty", new
        {
            id = dto.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString(),

        }, dto);
    }

    // DELETE: api/Counties/5
    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="id">Id of an entity</param>
    /// <returns>Status204</returns>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCounty(Guid id)
    {
        var county = await _appBLL.Counties.FirstOrDefaultAsync(id);
        if (county == null) return NotFound();

        _appBLL.Counties.Remove(county);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }
    /// <summary>
    /// Return a boolean based on whether or not an entity exists
    /// </summary>
    /// <param name="id">Entity id guid</param>
    /// <returns>boolean value</returns>
    
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private bool CountyExists(Guid id)
    {
        return _appBLL.Counties.Exists(id);
    }
}