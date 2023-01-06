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
public class VehicleModelsController : ControllerBase
{
    private readonly IAppBLL _appBLL;

    public VehicleModelsController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: api/VehicleModels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleModelDTO>>> GetVehicleModels()
    {
        return Ok(await _appBLL.VehicleModels.GetAllVehicleModelsWithoutVehicleMarksAsync());
    }

    // GET: api/VehicleModels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleModelDTO>> GetVehicleModel(Guid id)
    {
        var vehicleModel = await _appBLL.VehicleModels.FirstOrDefaultVehicleModelWithoutVehicleMarkAsync(id);

        if (vehicleModel == null) return NotFound();

        return vehicleModel;
    }

    // PUT: api/VehicleModels/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVehicleModel(Guid id, VehicleModelDTO vehicleModel)
    {
        if (id != vehicleModel.Id) return BadRequest();
        
        try
        {
            _appBLL.VehicleModels.Update(vehicleModel);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!VehicleModelExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/VehicleModels
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<VehicleModelDTO>> PostVehicleModel(VehicleModelDTO vehicleModel)
    {
        _appBLL.VehicleModels.Add(vehicleModel);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetVehicleModel", new {id = vehicleModel.Id}, vehicleModel);
    }

    // DELETE: api/VehicleModels/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehicleModel(Guid id)
    {
        var vehicleModel = await _appBLL.VehicleModels.FirstOrDefaultAsync(id);
        if (vehicleModel == null) return NotFound();

        _appBLL.VehicleModels.Remove(vehicleModel);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool VehicleModelExists(Guid id)
    {
        return _appBLL.VehicleModels.Exists(id);
    }
}