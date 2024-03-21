#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Public.DTO.v1.AdminArea;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppUser = App.BLL.DTO.Identity.AppUser;

namespace WebApp.ApiControllers.AdminArea;

/// <summary>
/// Api controller for drivers
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DriversController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Constructor for drivers api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.DriverDTO to Public.DTO.v1.AdminArea.Driver</param>
    /// <param name="userManager">For managing drivers</param>
    public DriversController(IAppBLL appBLL, IMapper mapper, UserManager<AppUser> userManager)
    {
        _appBLL = appBLL;
        _mapper = mapper;
        _userManager = userManager;
    }

    // GET: api/Drivers
    /// <summary>
    /// Gets all the drivers
    /// </summary>
    /// <returns>List of drivers with a statusCode 200OK or statusCode 403 or statusCode 401</returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<Driver>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<Driver>>> GetDrivers()
    {
        var res = await _appBLL.Drivers.GetAllDriversOrderedByLastNameAsync();
        return Ok(res.Select(x => _mapper.Map<Driver>(x)));
    }

    // GET: api/Drivers/5
    /// <summary>
    /// Returns driver based on id
    /// </summary>
    /// <param name="id">Driver id, Guid</param>
    /// <returns>Driver (TEntity) with statusCode 200 or statusCode 404 or statusCode 403 or statusCode 401</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Driver), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Driver>> GetDriver(Guid id)
    {
        var driver = await _appBLL.Drivers.FirstOrDefaultAsync(id);

        if (driver == null) return NotFound();

        return _mapper.Map<Driver>(driver);
    }

    // PUT: api/Drivers/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating a driver  
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="driver">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400</returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutDriver(Guid id, Driver driver)
    {
        if (id != driver.Id) return BadRequest();

        var driverLicenseCategories = await _appBLL.DriverAndDriverLicenseCategories
            .RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(driver.Id);
        try
        {
            var appUser = await _appBLL.AppUsers.GettingAppUserByAppUserIdAsync(driver.AppUserId);
            appUser.Email = appUser.Email;
            appUser.Gender = appUser.Gender;
            appUser.FirstName = appUser.FirstName;
            appUser.LastName = appUser.LastName;
            appUser.IsActive = appUser.IsActive;
            appUser.PhoneNumber = appUser.PhoneNumber;
            appUser.DateOfBirth = appUser.DateOfBirth;

            _appBLL.AppUsers.Update(appUser);
            await _appBLL.SaveChangesAsync();

            driver.Address = driver.Address;
            driver.CityId = driver.CityId;
            driver.DriverLicenseNumber = driver.DriverLicenseNumber;
            driver.DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate;
            driver.PersonalIdentifier = driver.PersonalIdentifier;
            driver.UpdatedBy = User.GettingUserEmail();
            driver.UpdatedAt = DateTime.Now.ToUniversalTime();

            var driverDto = _mapper.Map<DriverDTO>(driver);
            
            _appBLL.Drivers.Update(driverDto);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DriverExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Drivers/5
    /// <summary>
    /// Deletes an entity
    /// </summary>
    /// <param name="id">Id of an entity</param>
    /// <returns>Status204</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteDriver(Guid id)
    {
        var driver = await _appBLL.Drivers.FirstOrDefaultAsync(id);
        if (driver == null) return NotFound();

        await _appBLL.DriverAndDriverLicenseCategories
            .RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(driver.Id);
        
        var appUser = await _appBLL.AppUsers.GettingAppUserByAppUserIdAsync(driver.AppUserId);
        var claims = await _userManager.GetClaimsAsync(appUser);
        await _userManager.RemoveClaimsAsync(appUser, claims);
        var roles = await _userManager.GetRolesAsync(appUser);
        await _userManager.RemoveFromRolesAsync(appUser, roles);
        await _userManager.DeleteAsync(appUser);
        
        _appBLL.Drivers.Remove(driver);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Return a boolean based on whether or not an entity exists
    /// </summary>
    /// <param name="id">Entity id guid</param>
    /// <returns>boolean value</returns>
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private bool DriverExists(Guid id)
    {
        return _appBLL.Drivers.Exists(id);
    }
}