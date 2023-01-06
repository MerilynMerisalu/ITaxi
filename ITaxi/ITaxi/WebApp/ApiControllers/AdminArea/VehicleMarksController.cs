#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[Route("api/AdminArea/[controller]")]
[ApiController]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class VehicleMarksController : ControllerBase
{
    private readonly IAppBLL _appBLL;

    public VehicleMarksController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: api/VehicleMarks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleMarkDTO>>> GetVehicleMarks()
    {
        return Ok(await _appBLL.VehicleMarks.GetAllVehicleMarkOrderedAsync());
    }

    // GET: api/VehicleMarks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleMarkDTO>> GetVehicleMark(Guid id)
    {
        var vehicleMark = await _appBLL.VehicleMarks.FirstOrDefaultAsync(id);

        if (vehicleMark == null) return NotFound();

        return vehicleMark;
    }

    // PUT: api/VehicleMarks/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVehicleMark(Guid id, VehicleMarkDTO vehicleMark)
    {
        if (id != vehicleMark.Id) return BadRequest();

        try
        {
            _appBLL.VehicleMarks.Update(vehicleMark);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VehicleMarkExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/VehicleMarks
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<VehicleMarkDTO>> PostVehicleMark(VehicleMarkDTO vehicleMark)
    {
        _appBLL.VehicleMarks.Add(vehicleMark);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetVehicleMark", new {id = vehicleMark.Id}, vehicleMark);
    }

    // DELETE: api/VehicleMarks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehicleMark(Guid id)
    {
        var vehicleMark = await _appBLL.VehicleMarks.FirstOrDefaultAsync(id);
        if (vehicleMark == null) return NotFound();

        _appBLL.VehicleMarks.Remove(vehicleMark);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool VehicleMarkExists(Guid id)
    {
        return _appBLL.VehicleMarks.Exists(id);
    }
}