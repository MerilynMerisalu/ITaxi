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
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DrivesController : ControllerBase
{
    private readonly IAppBLL _appBLL;

    public DrivesController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
        
    }

    // GET: api/Drives
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DriveDTO>>> GetDrives()
    {
        return Ok(await _appBLL.Drives.GetAllDrivesWithoutIncludesAsync());
    }

    // GET: api/Drives/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DriveDTO>> GetDrive(Guid id)
    {
        var drive = await _appBLL.Drives.GettingDriveWithoutIncludesAsync(id);

        if (drive == null) return NotFound();

        return drive;
    }

    // PUT: api/Drives/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDrive(Guid id, DriveDTO drive)
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

    // POST: api/Drives
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<DriveDTO>> PostDrive(DriveDTO drive)
    {
        _appBLL.Drives.Add(drive);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetDrive", new {id = drive.Id}, drive);
    }

    // DELETE: api/Drives/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDrive(Guid id)
    {
        var drive = await _appBLL.Drives.FirstOrDefaultAsync(id);
        if (drive == null) return NotFound();

        _appBLL.Drives.Remove(drive);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool DriveExists(Guid id)
    {
        return _appBLL.Drives.Exists(id);
    }
}