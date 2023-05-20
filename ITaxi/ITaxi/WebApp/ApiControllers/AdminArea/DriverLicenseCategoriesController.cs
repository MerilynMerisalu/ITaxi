#nullable enable
using App.Contracts.BLL;
using App.Public.DTO.v1.AdminArea;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.BLL.DTO.AdminArea;
namespace WebApp.ApiControllers.AdminArea;

/// <summary>
/// Api controller for driver license categories
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DriverLicenseCategoriesController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for driver license categories api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.DriverLicenseCategoryDTO to Public.DTO.v1.AdminArea.DriverLicenseCategory</param>
    public DriverLicenseCategoriesController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/DriverLicenseCategories
    /// <summary>
    /// Gets all the driver license categories
    /// </summary>
    /// <returns>List of driver license categories with a statusCode 200OK or statusCode 403 or statusCode 401</returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<DriverLicenseCategory>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<DriverLicenseCategory>>> GetDriverLicenseCategories()
    {
        var res = await _appBLL.DriverLicenseCategories
            .GetAllDriverLicenseCategoriesOrderedAsync();
        return Ok(res.Select(dlc=> _mapper.Map<DriverLicenseCategory>(dlc)));
    }

    // GET: api/DriverLicenseCategories/5
    /// <summary>
    /// Returns driver license categories based on id
    /// </summary>
    /// <param name="id">Driver license category id, Guid</param>
    /// <returns>DriverLicenseCategory (TEntity) with statusCode 200 or statusCode 404 or statusCode 403 or statusCode 401</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(DriverLicenseCategory), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<DriverLicenseCategory>> GetDriverLicenseCategory(Guid id)
    {
        var driverLicenseCategory = await _appBLL.DriverLicenseCategories.FirstOrDefaultAsync(id);

        if (driverLicenseCategory == null) return NotFound();

        return _mapper.Map<DriverLicenseCategory>(driverLicenseCategory);
    }

    // PUT: api/DriverLicenseCategories/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating a driver license category 
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="driverLicenseCategory">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400</returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutDriverLicenseCategory(Guid id, DriverLicenseCategory driverLicenseCategory)
    {
        var driverLicenseCategoryDTO = await _appBLL.DriverLicenseCategories.FirstOrDefaultAsync(id);
        if (driverLicenseCategoryDTO == null)
        {
            return NotFound();
        }
        
        driverLicenseCategoryDTO.DriverLicenseCategoryName = driverLicenseCategory.DriverLicenseCategoryName;
        driverLicenseCategoryDTO.UpdatedBy = User.Identity!.Name;
        driverLicenseCategoryDTO.UpdatedAt = DateTime.Now;
        _appBLL.DriverLicenseCategories.Update(driverLicenseCategoryDTO);
        await _appBLL.SaveChangesAsync();
        return NoContent();
    }

    // POST: api/DriverLicenseCategories
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Creating a new driver license category 
    /// </summary>
    /// <param name="driverLicenseCategory">Driver license category with properties</param>
    /// <returns>Status201Created with an entity</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DriverLicenseCategory), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<DriverLicenseCategory>> PostDriverLicenseCategory(
        [FromBody]DriverLicenseCategory driverLicenseCategory)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }

        var dto = _mapper.Map<DriverLicenseCategoryDTO>(driverLicenseCategory);
        
        dto.Id = Guid.NewGuid();
        dto.DriverLicenseCategoryName = driverLicenseCategory.DriverLicenseCategoryName;
        dto.CreatedBy = User.GettingUserEmail();
        dto.UpdatedBy = User.GettingUserEmail();
        dto.CreatedAt = DateTime.Now.ToUniversalTime();
        dto.UpdatedAt = DateTime.Now.ToUniversalTime();

        _appBLL.DriverLicenseCategories.Add(dto);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetDriverLicenseCategory", new
        {
            id = dto.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString(),

        }, dto);
    }

    // DELETE: api/DriverLicenseCategories/5
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
    
    public async Task<IActionResult> DeleteDriverLicenseCategory(Guid id)
    {
        var driverLicenseCategory = await _appBLL.DriverLicenseCategories.FirstOrDefaultAsync(id);
        if (driverLicenseCategory == null) return NotFound();

        _appBLL.DriverLicenseCategories.Remove(driverLicenseCategory);
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
    private bool DriverLicenseCategoryExists(Guid id)
    {
        return _appBLL.DriverLicenseCategories.Exists(id);
    }
}