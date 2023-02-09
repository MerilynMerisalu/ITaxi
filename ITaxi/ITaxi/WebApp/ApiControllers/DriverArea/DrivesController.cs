#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.DAL;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.ApiControllers.DriverArea;

[Route("api/DriverArea/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, Driver", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DrivesController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;

    public DrivesController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: api/Drives
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DriveDTO>>> GetDrives()
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var res = await _uow.Drives.GettingAllOrderedDrivesWithIncludesAsync(userId, roleName);

        return Ok(res);
    }

    // GET: api/Drives/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DriveDTO>> GetDrive(Guid id)
    {
        var userId = User.GettingUserId();
        var roleName = User.GettingUserRoleName();
        var drive = await _uow.Drives.GettingFirstDriveAsync(id, userId, roleName);

        if (drive == null) return NotFound();



        return Ok(drive);
    }


// PUT: api/Drives/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /*[HttpPut("{id}")]
    public async Task<IActionResult> PutDrive(Guid id, Drive drive)
    {
        if (id != drive.Id) return BadRequest();


        try
        {
            _uow.Drives.Update(drive);
            await _uow.SaveChangesAsync();
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
        _uow.Drives.Add(drive);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetDrive", new {id = drive.Id}, drive);
    }
    */

    // DELETE: api/Drives/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDrive(Guid id)
    {
        var drive = await _uow.Drives.FirstOrDefaultAsync(id);
        if (drive == null) return NotFound();

        _uow.Drives.Remove(drive);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool DriveExists(Guid id)
    {
        return _uow.Drives.Exists(id);
    }
}