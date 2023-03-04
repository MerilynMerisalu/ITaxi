#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
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
public class AdminsController : ControllerBase
{
    private readonly IAppBLL _appBLL;

    public AdminsController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: api/Admins
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AdminDTO>>> GetAdmins()
    {
        var res = await _appBLL.Admins.GetAllAdminsOrderedByLastNameAsync();
        
        return Ok(res);
    }

    // GET: api/Admins/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AdminDTO>> GetAdmin(Guid id)
    {
        var admin = await _appBLL.Admins.FirstOrDefaultAsync(id);

        if (admin == null) return NotFound();
        
        
        return admin;
    }

    // PUT: api/Admins/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAdmin(Guid id, AdminDTO adminDTO)
    {
        if (id != adminDTO.Id) return BadRequest();

        try
        {
            
            adminDTO.CreatedBy = User.GettingUserEmail();
            adminDTO.UpdatedBy = User.GettingUserEmail();
            _appBLL.Admins.Update(adminDTO);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AdminExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Admins
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<AdminDTO>> PostAdmin([FromBody]AdminDTO adminDTO)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }
        adminDTO.CreatedBy = User.GettingUserEmail();
        adminDTO.UpdatedBy = User.GettingUserEmail();
        _appBLL.Admins.Add(adminDTO);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetAdmin", new
        {
            id = adminDTO.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString(),
        }, adminDTO);
    }

    // DELETE: api/Admins/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdmin(Guid id)
    {
        var admin = await _appBLL.Admins.FirstOrDefaultAsync(id);
        if (admin == null) return NotFound();

        _appBLL.Admins.Remove(admin);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool AdminExists(Guid id)
    {
        return _appBLL.Admins.Exists(id);
    }
}