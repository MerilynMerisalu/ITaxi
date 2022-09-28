#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DisabilityTypesController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;

    public DisabilityTypesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: api/DisabilityTypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DisabilityType>>> GetDisabilityTypes()
    {
        return Ok(await _uow.DisabilityTypes.GetAllDisabilityTypeDtoAsync());
    }

    // GET: api/DisabilityTypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DisabilityType>> GetDisabilityType(Guid id)
    {
        var disabilityType = await _uow.DisabilityTypes.FirstOrDefaultAsync(id);

        if (disabilityType == null) return NotFound();

        return disabilityType;
    }

    // PUT: api/DisabilityTypes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDisabilityType(Guid id, DisabilityType disabilityType)
    {
        if (id != disabilityType.Id) return BadRequest();


        try
        {
            _uow.DisabilityTypes.Update(disabilityType);
            await _uow.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DisabilityTypeExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/DisabilityTypes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<DisabilityType>> PostDisabilityType(DisabilityType disabilityType)
    {
        _uow.DisabilityTypes.Add(disabilityType);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetDisabilityType", new {id = disabilityType.Id}, disabilityType);
    }

    // DELETE: api/DisabilityTypes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDisabilityType(Guid id)
    {
        var disabilityType = await _uow.DisabilityTypes.FirstOrDefaultAsync(id);
        if (disabilityType == null) return NotFound();

        _uow.DisabilityTypes.Remove(disabilityType);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool DisabilityTypeExists(Guid id)
    {
        return _uow.DisabilityTypes.Exists(id);
    }
}