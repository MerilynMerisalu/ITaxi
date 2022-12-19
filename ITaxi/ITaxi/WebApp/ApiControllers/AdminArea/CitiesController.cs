#nullable enable
using App.Contracts.DAL;
using App.Domain;
using App.DTO.AdminArea;
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
    private readonly IAppUnitOfWork _uow;

    public CitiesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: api/Cities
    [HttpGet]
    public async Task<ActionResult<IEnumerable<City>>> GetCities()
    {
        var cities = await _uow.Cities.GetAllOrderedCitiesWithoutCountyAsync();
        return Ok(cities);
    }

    // GET: api/Cities/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CityDTO>> GetCity(Guid id)
    {
        var city = await _uow.Cities.FirstOrDefaultCityWithoutCountyAsync(id);

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
            _uow.Cities.Update(city);
            await _uow.SaveChangesAsync();
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
    public async Task<ActionResult<City>> PostCity(CityDTO city)
    {
        _uow.Cities.Add(city);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetCity", new {id = city.Id}, city);
    }

    // DELETE: api/Cities/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(Guid id)
    {
        var city = await _uow.Cities.FirstOrDefaultAsync(id);
        if (city == null) return NotFound();

        _uow.Cities.Remove(city);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool CityExists(Guid id)
    {
        return _uow.Cities.Exists(id);
    }
}