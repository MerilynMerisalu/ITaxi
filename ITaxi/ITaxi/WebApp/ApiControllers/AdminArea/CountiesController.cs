#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers.AdminArea;

[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiController]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiVersion("1.0")]
public class CountiesController : ControllerBase
{
    private readonly IAppBLL _appBLL;

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
    [ProducesResponseType(typeof(IEnumerable<CountyDTO>), 
        StatusCodes.Status200OK)]
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
    public async Task<ActionResult<CountyDTO>> GetCounty(Guid id)
    {
        var county = await _appBLL.Counties.FirstOrDefaultAsync(id);

        if (county == null) return NotFound();
       


        return county;
    }

    // PUT: api/Counties/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
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
    [HttpPost]
    public async Task<ActionResult<CountyDTO>> PostCounty(CountyDTO county)
    {

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
            version = HttpContext.GetRequestedApiVersion()?.ToString() ?? "0",

        }, county);
    }

    // DELETE: api/Counties/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCounty(Guid id)
    {
        var county = await _appBLL.Counties.FirstOrDefaultAsync(id);
        if (county == null) return NotFound();

        _appBLL.Counties.Remove(county);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool CountyExists(Guid id)
    {
        return _appBLL.Counties.Exists(id);
    }
}