#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Contracts.DAL;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[Route("api/AdminArea/[controller]")]
[ApiController]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DisabilityTypesController : ControllerBase
{
    private readonly IAppBLL _appBLL;

    public DisabilityTypesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
        ;
    }

    // GET: api/DisabilityTypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DisabilityTypeDTO>>> GetDisabilityTypes()
    {
        return Ok(await _appBLL.DisabilityTypes.GetAllDisabilityTypeDtoAsync());
    }

    // GET: api/DisabilityTypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DisabilityTypeDTO>> GetDisabilityType(Guid id)
    {
        var disabilityType = await _appBLL.DisabilityTypes.FirstOrDefaultAsync(id);

        if (disabilityType == null) return NotFound();

        return disabilityType;
    }

    // PUT: api/DisabilityTypes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDisabilityType(Guid id, DisabilityTypeDTO disabilityType)
    {
        if (id != disabilityType.Id) return BadRequest();


        try
        {
            _appBLL.DisabilityTypes.Update(disabilityType);
            await _appBLL.SaveChangesAsync();
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
    public async Task<ActionResult<DisabilityTypeDTO>> PostDisabilityType(DisabilityTypeDTO disabilityType)
    {
        _appBLL.DisabilityTypes.Add(disabilityType);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetDisabilityType", new {id = disabilityType.Id}, disabilityType);
    }

    // DELETE: api/DisabilityTypes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDisabilityType(Guid id)
    {
        var disabilityType = await _appBLL.DisabilityTypes.FirstOrDefaultAsync(id);
        if (disabilityType == null) return NotFound();

        _appBLL.DisabilityTypes.Remove(disabilityType);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool DisabilityTypeExists(Guid id)
    {
        return _appBLL.DisabilityTypes.Exists(id);
    }
}