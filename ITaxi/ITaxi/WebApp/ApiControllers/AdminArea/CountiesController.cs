#nullable enable
using App.Contracts.DAL;
using App.DAL.DTO.AdminArea;
using App.Domain;
using App.Domain.DTO.AdminArea;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers.AdminArea;

[Route("api/AdminArea/[controller]")]
[ApiController]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CountiesController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;

    public CountiesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: api/Counties
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CountyDTO>>> GetCounties()
    {
        var res = await _uow.Counties.GetAllCountiesOrderedByCountyNameAsync();
        foreach (var county in res)
        {
            /*county.CreatedAt = county.CreatedAt.ToLocalTime();
            county.UpdatedAt = county.UpdatedAt.ToLocalTime();*/
        }
        return Ok(res);
    }

    // GET: api/Counties/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CountyDTO>> GetCounty(Guid id)
    {
        var county = await _uow.Counties.FirstOrDefaultAsync(id);

        if (county == null) return NotFound();
        /*county.CreatedAt = county.CreatedAt.ToLocalTime();
        county.UpdatedAt = county.UpdatedAt.ToLocalTime();
        */


        return county;
    }

    // PUT: api/Counties/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCounty(Guid id, /*County? county,*/ CountyDTO countyDTO)
    {

        var county = await _uow.Counties.FirstOrDefaultAsync(id);
        if (county == null)
        {
            return NotFound();
        }


        county.CountyName = countyDTO.CountyName;
        /*county.UpdatedBy = User.Identity!.Name;
        county.UpdatedAt = DateTime.Now.ToUniversalTime();*/
        _uow.Counties.Update(county);
        await _uow.SaveChangesAsync();



        return NoContent();
    }

    // POST: api/Counties
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<County>> PostCounty(CountyDTO county)
    {

        /*county.CreatedBy = User.Identity!.Name;
        county.CreatedAt = DateTime.Now.ToUniversalTime();
        county.UpdatedBy = User.Identity!.Name;
        county.UpdatedAt = DateTime.Now.ToUniversalTime();*/
        _uow.Counties.Add(county);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetCounty", new { id = county.Id }, county);
    }

    // DELETE: api/Counties/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCounty(Guid id)
    {
        var county = await _uow.Counties.FirstOrDefaultAsync(id);
        if (county == null) return NotFound();

        _uow.Counties.Remove(county);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool CountyExists(Guid id)
    {
        return _uow.Counties.Exists(id);
    }
}