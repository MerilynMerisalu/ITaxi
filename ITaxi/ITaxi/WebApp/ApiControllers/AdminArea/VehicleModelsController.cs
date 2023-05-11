#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Public.DTO.v1.AdminArea;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class VehicleModelsController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;

    public VehicleModelsController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/VehicleModels
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<VehicleModel>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<VehicleModel>>> GetVehicleModels()
    {
        var res = await _appBLL.VehicleModels.GetAllVehicleModelsWithoutVehicleMarksAsync();
        return Ok(res.Select(v => _mapper.Map<VehicleModel>(v)));
    }

    // GET: api/VehicleModels/5
    [HttpGet("{id}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(VehicleModel), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehicleModel>> GetVehicleModel(Guid id)
    {
        var vehicleModel = await _appBLL.VehicleModels.FirstOrDefaultVehicleModelWithoutVehicleMarkAsync(id);

        if (vehicleModel == null) return NotFound();

        return _mapper.Map<VehicleModel>(vehicleModel);
    }

    // PUT: api/VehicleModels/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutVehicleModel(Guid id, VehicleModel vehicleModel)
    {
        if (id != vehicleModel.Id) return BadRequest();
        var vehicleModelDto = await _appBLL.VehicleModels.FirstOrDefaultAsync(id);
        try
        {
            if (vehicleModelDto != null)
            {
                vehicleModelDto.VehicleMarkId = vehicleModel.VehicleMarkId;
                vehicleModelDto.VehicleModelName = vehicleModel.VehicleModelName;
                vehicleModelDto.UpdatedBy = User.GettingUserEmail();
                vehicleModelDto.CreatedAt = DateTime.Now.ToUniversalTime();
                _appBLL.VehicleModels.Update(vehicleModelDto);
            }

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
    [Produces("application/json")]
    [ProducesResponseType(typeof(VehicleModel), 
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]

    public async Task<ActionResult<VehicleModel>> PostVehicleModel([FromBody]VehicleModel vehicleModel)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }

        var vehicleModelDto = new VehicleModelDTO();
        vehicleModelDto.Id = Guid.NewGuid();
        vehicleModel.VehicleMarkId = vehicleModel.VehicleMarkId;
        vehicleModelDto.VehicleModelName = vehicleModel.VehicleModelName;
        vehicleModelDto.CreatedBy = User.GettingUserEmail();
        vehicleModelDto.CreatedAt = DateTime.Now.ToUniversalTime();
        vehicleModelDto.UpdatedBy = User.GettingUserEmail();
        vehicleModelDto.UpdatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.VehicleModels.Add(vehicleModelDto);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetVehicleModel", new
        {
            id = vehicleModel.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString(),
        }, vehicleModel);
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