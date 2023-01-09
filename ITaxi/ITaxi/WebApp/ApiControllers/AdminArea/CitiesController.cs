#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[Route("api/AdminArea/[controller]")]
[ApiController]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CitiesController : ControllerBase
{
    private readonly IAppBLL _appBLL;

    public CitiesController( IAppBLL appBLL)
    {
        _appBLL = appBLL;
        
    }

    // GET: api/Cities
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CityDTO>>> GetCities()
    {
        var cities = await _appBLL.Cities.GetAllOrderedCitiesWithoutCountyAsync();
        return Ok(cities);
    }

    // GET: api/Cities/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CityDTO>> GetCity(Guid id)
    {
        var city = await _appBLL.Cities.FirstOrDefaultCityWithoutCountyAsync(id);

        if (city == null) return NotFound();

        return city;
    }

    // PUT: api/Cities/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCity(Guid id, CityDTO city)
    {
        if (id != city.Id) return BadRequest();

        try
        {
            _appBLL.Cities.Update(city);
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
    [HttpPost]
    public async Task<ActionResult<CityDTO>> PostCity(CityDTO city)
    {
        _appBLL.Cities.Add(city);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetCity", new {id = city.Id}, city);
    }

    // DELETE: api/Cities/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(Guid id)
    {
        var city = await _appBLL.Cities.FirstOrDefaultCityWithoutCountyAsync(id);
        if (city == null) return NotFound();

        _appBLL.Cities.Remove(city);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool CityExists(Guid id)
    {
        return _appBLL.Cities.Exists(id);
    }
}