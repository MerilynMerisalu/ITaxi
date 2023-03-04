#nullable enable
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[ApiController]
[Route("api/v{version:apiVersion}/AdminArea/[controller]")]
[ApiVersion("1.0")]
[Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CustomersController : ControllerBase
{
    private readonly IAppBLL _appBLL;

    public CustomersController(IAppBLL appBLL)
    {
        _appBLL = appBLL;
    }

    // GET: api/Customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        return Ok(await _appBLL.Customers.GettingAllOrderedCustomersWithoutIncludesAsync());
    }

    // GET: api/Customers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDTO>> GetCustomer(Guid id)
    {
        var customer = await _appBLL.Customers.GettingCustomerByIdWithoutIncludesAsync(id);

        if (customer == null) return NotFound();

        return customer;
    }

    // PUT: api/Customers/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(Guid id, CustomerDTO customer)
    {
        if (id != customer.Id) return BadRequest();

        try
        {
            _appBLL.Customers.Update(customer);
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

    // POST: api/Customers
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<CustomerDTO>> PostCustomer([FromBody]CustomerDTO customer)
    {
        if (HttpContext.GetRequestedApiVersion() == null)
        {
            return BadRequest("Api version is mandatory");
        }
        _appBLL.Customers.Add(customer);
        await _appBLL.SaveChangesAsync();

        return CreatedAtAction("GetCustomer", new
        {
            id = customer.Id,
            version = HttpContext.GetRequestedApiVersion()!.ToString(), 
        }, customer);
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        var customer = await _appBLL.Customers.GettingCustomerByIdWithoutIncludesAsync(id);
        if (customer == null) return NotFound();

        _appBLL.Customers.Remove(customer);
        await _appBLL.SaveChangesAsync();

        return NoContent();
    }

    private bool CustomerExists(Guid id)
    {
        return _appBLL.Customers.Exists(id);
    }
}