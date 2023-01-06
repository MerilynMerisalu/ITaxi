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
[Authorize(Roles = "Admin" , AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class VehicleTypesController : ControllerBase
{
    private readonly IAppBLL _appBLL;

    public VehicleTypesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: api/VehicleTypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleTypeDTO>>> GetVehicleTypes()
    {
        return Ok(await _appBLL.VehicleTypes.GetAllVehicleTypesDTOAsync());
    }

    // GET: api/VehicleTypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleTypeDTO>> GetVehicleType(Guid id)
    {
        var vehicleType = await _appBLL.VehicleTypes.FirstOrDefaultAsync(id);

        if (vehicleType == null) return NotFound();

        return vehicleType;
    }

    // PUT: api/VehicleTypes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVehicleType(Guid id, VehicleTypeDTO vehicleType)
    {
        if (id != vehicleType.Id) return BadRequest();


        try
        {
            _appBLL.VehicleTypes.Update(vehicleType);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VehicleTypeExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/VehicleTypes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<VehicleTypeDTO>> PostVehicleType(VehicleTypeDTO vehicleType)
    {
        _appBLL.VehicleTypes.Add(vehicleType);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetVehicleType", new {id = vehicleType.Id}, vehicleType);
    }

    // DELETE: api/VehicleTypes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehicleType(Guid id)
    {
        var vehicleType = await _appBLL.VehicleTypes.FirstOrDefaultAsync(id);
        if (vehicleType == null) return NotFound();

        _appBLL.VehicleTypes.Remove(vehicleType);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool VehicleTypeExists(Guid id)
    {
        return _appBLL.VehicleTypes.Exists(id);
    }
}