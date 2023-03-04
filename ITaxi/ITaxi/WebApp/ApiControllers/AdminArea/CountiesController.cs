#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
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

    /// <summary>
    ///  constructor for counties
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    public CountiesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
        
    }

    // GET: api/counties
    /// <summary>
    /// Gets all the counties 
    /// </summary>
    /// <returns>List of counties</returns>
    [HttpGet]
    [Produces("application/json")]
    //[Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<CountyDTO>), 
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    
    public async Task<ActionResult<IEnumerable<CountyDTO>>> GetCounties()
    {
        var res = await _appBLL.Counties.GetAllCountiesOrderedByCountyNameAsync();

        return Ok(res);
    }

    // GET: api/Counties/5
    /// <summary>
    /// Returns county based on id
    /// </summary>
    /// <param name="id">County id, Guid</param>
    /// <returns>County(TEntity)</returns>
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CountyDTO), 
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    
    public async Task<ActionResult<CountyDTO>> GetCounty(Guid id)
    {
        var county = await _appBLL.Counties.FirstOrDefaultAsync(id);

        if (county == null) return NotFound();
       


        return county;
    }

    // PUT: api/Counties/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating an county 
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="countyDTO">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or Statuscode 404 or statusCode 401 </returns>
    [HttpPut("{id}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CountyDTO), 
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutCounty(Guid id, CountyDTO countyDTO)
    {
        var county = await _appBLL.Counties.FirstOrDefaultAsync(id);
        if (county == null)
        {
            return NotFound();
        }


        county.CountyName = countyDTO.CountyName;
        county.UpdatedBy = User.Identity!.Name;
        county.UpdatedAt = DateTime.Now;
        _appBLL.Counties.Update(county);
        await _appBLL.SaveChangesAsync();



        return NoContent();
    }

    // POST: api/Counties
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Creating a new county
    /// </summary>
    /// <param name="county">CountyDTO with properties</param>
    /// <returns>Status201Created with an entity</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(CountyDTO), 
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    
    public async Task<ActionResult<CountyDTO>> PostCounty([FromBody] CountyDTO county)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }
        county.Id = Guid.NewGuid();
        county.CreatedBy = User.GettingUserEmail();
        county.UpdatedBy = User.GettingUserEmail();
        county.CreatedAt = DateTime.Now.ToUniversalTime();
        county.UpdatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.Counties.Add(county);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetCounty", new
        {
            id = county.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString(),

        }, county);
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

    [HttpDelete("{id}")]
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