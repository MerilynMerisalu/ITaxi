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
/// Api controller for customers
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CustomersController : ControllerBase
{
    private readonly IAppBLL _appBLL;
    private readonly IMapper _mapper;
    /// <summary>
    /// Constructor for customers api controller
    /// </summary>
    /// <param name="appBLL">AppBLL</param>
    /// <param name="mapper">Mapper for mapping App.BLL.DTO.AdminArea.CustomerDTO to Public.DTO.v1.AdminArea.Customer</param>
    public CustomersController(IAppBLL appBLL, IMapper mapper)
    {
        _appBLL = appBLL;
        _mapper = mapper;
    }

    // GET: api/Customers
    /// <summary>
    /// Gets all the customers
    /// </summary>
    /// <returns>List of customers with a statusCode 200OK or statusCode 403 or statusCode 401</returns>
    [HttpGet]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType( typeof( IEnumerable<Customer>), StatusCodes.Status200OK )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        var res = await _appBLL.Customers.GettingAllOrderedCustomersWithoutIncludesAsync();
        return Ok(res.Select(c=> _mapper.Map<Customer>(c)));
    }

    // GET: api/Customers/5
    /// <summary>
    /// Returns customer based on id
    /// </summary>
    /// <param name="id">Customer id, Guid</param>
    /// <returns>Customer (TEntity) with statusCode 200 or statusCode 404 or statusCode 403 or statusCode 401</returns>
    [HttpGet("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK )] 
    [ProducesResponseType( StatusCodes.Status404NotFound )] 
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Customer>> GetCustomer(Guid id)
    {
        var customer = await _appBLL.Customers.GettingCustomerByIdWithoutIncludesAsync(id);

        if (customer == null) return NotFound();

        return _mapper.Map<Customer>(customer);
    }

    // PUT: api/Customers/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Updating a customer
    /// </summary>
    /// <param name="id">An id of the entity which is updated</param>
    /// <param name="customer">DTO which holds the values</param>
    /// <returns>StatusCode 204 or StatusCode 403 or StatusCode 404 or StatusCode 401 or StatusCode 400</returns>
    [HttpPut("{id:guid}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutCustomer(Guid id, Customer customer)
    {
        if (id != customer.Id) return BadRequest();

        var appUser = await _appBLL.AppUsers.GettingAppUserByAppUserIdAsync(customer.AppUserId);
        var custumerDTO = await _appBLL.Customers.GettingCustomerByIdWithoutIncludesAsync(id);
        try
        {
            appUser.FirstName = customer.AppUser!.FirstName;
            appUser.LastName = customer.AppUser.LastName;
            appUser.Email = customer.AppUser.Email;
            appUser.Gender = customer.AppUser.Gender;
            appUser.PhoneNumber = customer.AppUser.PhoneNumber;
            appUser.DateOfBirth = customer.AppUser.DateOfBirth;
            _appBLL.AppUsers.Update(appUser);
            if (custumerDTO != null)
            {
                custumerDTO.DisabilityTypeId = customer.DisabilityTypeId;
                custumerDTO.UpdatedBy = User.GettingUserEmail();
                custumerDTO.UpdatedAt = DateTime.Now.ToUniversalTime();
                _appBLL.Customers.Update(custumerDTO);
            }

            await _appBLL.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CustomerExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    
    // DELETE: api/Customers/5
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
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        var customer = await _appBLL.Customers.GettingCustomerByIdWithoutIncludesAsync(id);
        if (customer == null) return NotFound();

        _appBLL.Customers.Remove(customer);
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
    private bool CustomerExists(Guid id)
    {
        return _appBLL.Customers.Exists(id);
    }
}