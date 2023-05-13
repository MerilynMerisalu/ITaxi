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
public class VehicleTypesController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;
    public VehicleTypesController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/VehicleTypes
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<VehicleType>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<VehicleType>>> GetVehicleTypes()
    {
        var res = await _appBLL.VehicleTypes.GetAllVehicleTypesOrderedAsync();
        return Ok(res.Select(v => _mapper.Map<VehicleType>(v)));
    }

    // GET: api/VehicleTypes/5
    [HttpGet("{id}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(VehicleType), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehicleType>> GetVehicleType(Guid id)
    {
        var vehicleType = await _appBLL.VehicleTypes.FirstOrDefaultAsync(id);

        if (vehicleType == null) return NotFound();

        return _mapper.Map<VehicleType>(vehicleType);
    }

    // PUT: api/VehicleTypes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutVehicleType(Guid id, VehicleType vehicleType)
    {
        if (id != vehicleType.Id) return BadRequest();

        
        try
        {
            var vehicleTypeDto = await _appBLL.VehicleTypes.FirstOrDefaultAsync(id);

            if (vehicleTypeDto != null)
            {
                vehicleTypeDto.VehicleTypeName = vehicleType.VehicleTypeName;
                vehicleTypeDto.UpdatedAt = DateTime.Now.ToUniversalTime();
                vehicleTypeDto.UpdatedBy = User.GettingUserEmail();
            }

            if (vehicleTypeDto != null) _appBLL.VehicleTypes.Update(vehicleTypeDto);
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
    [Produces("application/json")]
    [ProducesResponseType(typeof(VehicleType), 
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]

    public async Task<ActionResult<VehicleType>> PostVehicleType([FromBody]VehicleType vehicleType)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory!");
        }

        var vehicleTypeDto = new VehicleTypeDTO();
        vehicleTypeDto.Id = Guid.NewGuid();
        vehicleTypeDto.VehicleTypeName = vehicleType.VehicleTypeName;
        vehicleTypeDto.CreatedBy = User.GettingUserEmail();
        vehicleTypeDto.CreatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.VehicleTypes.Add(vehicleTypeDto);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetVehicleType", new {id = vehicleType.Id, 
            version = HttpContext.GetRequestedApiVersion()!.ToString(),}, vehicleType);
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