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
public class VehicleModelsController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;

    public VehicleModelsController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: api/VehicleModels
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleModel>>> GetVehicleModels()
    {
        return Ok(await _uow.VehicleModels.GetAllVehicleModelsWithoutVehicleMarksAsync());
    }

    // GET: api/VehicleModels/5
    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleModel>> GetVehicleModel(Guid id)
    {
        var vehicleModel = await _uow.VehicleModels.FirstOrDefaultVehicleModelWithoutVehicleMarkAsync(id);

        if (vehicleModel == null) return NotFound();

        return vehicleModel;
    }

    // PUT: api/VehicleModels/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVehicleModel(Guid id, VehicleModel vehicleModel)
    {
        if (id != vehicleModel.Id) return BadRequest();


        try
        {
            _uow.VehicleModels.Update(vehicleModel);
            await _uow.SaveChangesAsync();
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
    public async Task<ActionResult<VehicleModel>> PostVehicleModel(VehicleModel vehicleModel)
    {
        _uow.VehicleModels.Add(vehicleModel);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetVehicleModel", new {id = vehicleModel.Id}, vehicleModel);
    }

    // DELETE: api/VehicleModels/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehicleModel(Guid id)
    {
        var vehicleModel = await _uow.VehicleModels.FirstOrDefaultAsync(id);
        if (vehicleModel == null) return NotFound();

        _uow.VehicleModels.Remove(vehicleModel);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool VehicleModelExists(Guid id)
    {
        return _uow.VehicleModels.Exists(id);
    }
}