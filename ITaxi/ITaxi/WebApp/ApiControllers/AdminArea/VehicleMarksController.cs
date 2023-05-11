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
public class VehicleMarksController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;
    public VehicleMarksController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/VehicleMarks
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<VehicleMark>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<VehicleMark>>> GetVehicleMarks()
    {

        var res = await _appBLL.VehicleMarks.GetAllVehicleMarkOrderedAsync();
        return Ok(res.Select(v => _mapper.Map<VehicleMark>(v)));
    }

    // GET: api/VehicleMarks/5
    [HttpGet("{id}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(VehicleMark), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<VehicleMark>> GetVehicleMark(Guid id)
    {
        var vehicleMark = await _appBLL.VehicleMarks.FirstOrDefaultAsync(id);

        if (vehicleMark == null) return NotFound();

        return _mapper.Map<VehicleMark>(vehicleMark);
    }

    // PUT: api/VehicleMarks/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutVehicleMark(Guid id, VehicleMark vehicleMark)
    {
        if (id != vehicleMark.Id) return BadRequest();

        var vehicleMarkDto = await _appBLL.VehicleMarks.FirstOrDefaultAsync(id);
        
        try
        {
            if (vehicleMarkDto != null)
            {
                vehicleMarkDto.VehicleMarkName = vehicleMark.VehicleMarkName;
                vehicleMarkDto.UpdatedBy = User.GettingUserEmail();
                vehicleMarkDto.UpdatedAt = DateTime.Now.ToUniversalTime();
            }

            if (vehicleMarkDto != null) _appBLL.VehicleMarks.Update(vehicleMarkDto);
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
    public async Task<ActionResult<VehicleMark>> PostVehicleMark([FromBody]VehicleMark vehicleMark)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }

        var vehicleMarkDto = new VehicleMarkDTO();
        vehicleMarkDto.VehicleMarkName = vehicleMark.VehicleMarkName;
        vehicleMarkDto.CreatedBy = User.GettingUserEmail();
        vehicleMarkDto.CreatedAt = DateTime.Now.ToUniversalTime();
        vehicleMarkDto.UpdatedBy = User.GettingUserEmail();
        vehicleMarkDto.UpdatedAt = DateTime.Now.ToUniversalTime();
        _appBLL.VehicleMarks.Add(vehicleMarkDto);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetVehicleMark", new
        {
            id = vehicleMark.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString() ,
        }, vehicleMark);
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