#nullable enable

using App.Contracts.BLL;
using App.Public.DTO.v1.DriverArea;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;

namespace WebApp.ApiControllers.DriverArea;

[ApiController]
[Route("api/v{version:apiVersion}/DriverArea/[controller]")]
[Authorize(Roles = "Admin, Driver", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DrivesController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;
    public DrivesController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/Drives
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Drive>>> GetDrives()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _appBLL.Drives.GettingAllOrderedDrivesWithIncludesAsync(userId, roleName);

        var results = res.Select(d => _mapper.Map<Drive>(d));
        return Ok(results);
    }

    // GET: api/Drives/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Drive>> GetDrive(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var drive = await _appBLL.Drives.GettingFirstDriveAsync(id, userId, roleName);

        if (drive == null) return NotFound();
        
        return Ok(_mapper.Map<Drive>(drive));
    }


// PUT: api/Drives/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /*[HttpPut("{id}")]
    public async Task<IActionResult> PutDrive(Guid id, Drive drive)
    {
        if (id != drive.Id) return BadRequest();


        try
        {
            _appBLL.Drives.Update(drive);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DriveExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }
    */

    // POST: api/Drives
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /*[HttpPost]
    public async Task<ActionResult<Drive>> PostDrive(Drive drive)
    {
        _appBLL.Drives.Add(drive);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetDrive", new {id = drive.Id}, drive);
    }
    */

    // DELETE: api/Drives/5
    /*[HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDrive(Guid id)
    {
        var drive = await _appBLL.Drives.FirstOrDefaultAsync(id);
        if (drive == null) return NotFound();

        _appBLL.Drives.Remove(drive);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }*/
    [HttpPut("{id}")]
    
    public async Task<IActionResult> Accept (Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var drive = await _appBLL.Drives.GettingFirstDriveAsync(id, userId, roleName, false);
        if (drive != null)
        {
            await _appBLL.Drives.AcceptingDriveAsync(drive.Id, userId, roleName);
        }

        return NoContent();
    }
    
    private bool DriveExists(Guid id)
    {
        return _appBLL.Drives.Exists(id);
    }
    
    /*[Route("Print")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<string> Print()
    {
        var roleName = User.GettingUserRoleName();
        var userId = User.GettingUserId();
    
        var drives = await _appBLL.Drives.PrintAsync( userId, roleName );

        var mappedDrives = drives.Select(d => _mapper.Map<Drive>(d));
        return Url.ActionLink("Print")!  ; ;

    }
    */
    
    

}