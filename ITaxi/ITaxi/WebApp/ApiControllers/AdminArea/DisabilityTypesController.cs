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

/// <summary>
/// Api controller for disability types
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DisabilityTypesController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;
    
    /// <summary>
    ///  /// <summary>
    /// Constructor for disability types api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.DisabilityTypeDTO to Public.DTO.v1.AdminArea.DisabilityType</param>
    /// </summary>
    public DisabilityTypesController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/counties
    /// <summary>
    /// Gets all the disability types 
    /// </summary>
    /// <returns>List of disability types with a statusCode 200OK or statusCode 403 or statusCode 401 </returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<DisabilityType>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [AllowAnonymous]
    
    public async Task<ActionResult<IEnumerable<DisabilityType>>> GetDisabilityTypes()
    {
        var res =await _appBLL.DisabilityTypes.GetAllDisabilityTypeDtoAsync();
        return Ok(res.Select(x => 
            _mapper.Map<DisabilityType>(x)).ToList());
    }

    // GET: api/DisabilityTypes/5
    /// <summary>
    /// Returns disability type based on id
    /// </summary>
    /// <param name="id">Disability type id, Guid</param>
    /// <returns>Disability type (TEntity) with statusCode 200 or statusCode 404 or statusCode 403 or statusCode 401   </returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(DisabilityType), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [AllowAnonymous]
    public async Task<ActionResult<DisabilityType>> GetDisabilityType(Guid id)
    {
        var disabilityType = await _appBLL.DisabilityTypes.FirstOrDefaultAsync(id);

        if (disabilityType == null) return NotFound();

        return _mapper.Map<DisabilityType>(disabilityType);
    }

    // PUT: api/DisabilityTypes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating an disability type
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="disabilityType">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400 </returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutDisabilityType(Guid id, DisabilityType disabilityType)
    {
        var disabilityTypeDTO = await _appBLL.DisabilityTypes.FirstOrDefaultAsync(id);
        if (disabilityTypeDTO == null)
        {
            return NotFound();
        }
        
        try
        {
            disabilityTypeDTO.DisabilityTypeName = disabilityType.DisabilityTypeName;
            disabilityTypeDTO.UpdatedBy = User.GettingUserEmail();
            disabilityTypeDTO.UpdatedAt = DateTime.Now.ToUniversalTime();
            
            _appBLL.DisabilityTypes.Update(disabilityTypeDTO);
            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DisabilityTypeExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/DisabilityTypes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Creating a new disability type
    /// </summary>
    /// <param name="disabilityType">Disability type with properties</param>
    /// <returns>Status201Created with an entity</returns>
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(typeof(DisabilityType), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<DisabilityType>> PostDisabilityType
        ([FromBody] DisabilityType disabilityType)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory!");
        }

        var dto = _mapper.Map<DisabilityTypeDTO>(disabilityType);
        dto.Id = Guid.NewGuid();
        dto.DisabilityTypeName = disabilityType.DisabilityTypeName;
        _appBLL.DisabilityTypes.Add(dto);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetDisabilityType", new
        {
            id = disabilityType.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString(),
        }, disabilityType);
    }

    // DELETE: api/DisabilityTypes/5
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
    public async Task<IActionResult> DeleteDisabilityType(Guid id)
    {
        var disabilityType = await _appBLL.DisabilityTypes.FirstOrDefaultAsync(id);
        if (disabilityType == null) return NotFound();

        _appBLL.DisabilityTypes.Remove(disabilityType);
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
    private bool DisabilityTypeExists(Guid id)
    {
        return _appBLL.DisabilityTypes.Exists(id);
    }
}