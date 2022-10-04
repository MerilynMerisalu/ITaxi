#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[Route("api/adminArea/[controller]")]
[ApiController]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AdminsController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;

    public AdminsController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: api/Admins
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Admin>>> GetAdmins()
    {
        var res = await _uow.Admins.GetAllAdminsOrderedByLastNameAsync();
        foreach (var admin in res)
        {
            admin.CreatedAt = admin.CreatedAt.ToLocalTime();
            admin.UpdatedAt = admin.UpdatedAt.ToLocalTime();
        }
        return Ok(res);
    }

    // GET: api/Admins/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Admin>> GetAdmin(Guid id)
    {
        var admin = await _uow.Admins.FirstOrDefaultAsync(id);

        if (admin == null) return NotFound();
        
        admin.CreatedAt = admin.CreatedAt.ToLocalTime();
        admin.UpdatedAt = admin.UpdatedAt.ToLocalTime();

        return admin;
    }

    // PUT: api/Admins/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAdmin(Guid id, Admin admin)
    {
        if (id != admin.Id) return BadRequest();


        try
        {
            _uow.Admins.Update(admin);
            await _uow.SaveChangesAsync();
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
    public async Task<ActionResult<Admin>> PostAdmin(Admin admin)
    {
        _uow.Admins.Add(admin);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetAdmin", new {id = admin.Id}, admin);
    }

    // DELETE: api/Admins/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdmin(Guid id)
    {
        var admin = await _uow.Admins.FirstOrDefaultAsync(id);
        if (admin == null) return NotFound();

        _uow.Admins.Remove(admin);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool AdminExists(Guid id)
    {
        return _uow.Admins.Exists(id);
    }
}