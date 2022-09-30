#nullable enable
using App.Contracts.DAL;
using App.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers.AdminArea;

[Route("api/AdminArea/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IAppUnitOfWork _uow;

    public CustomersController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }

    // GET: api/Customers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
    {
        return Ok(await _uow.Customers.GettingAllOrderedCustomersWithoutIncludesAsync());
    }

    // GET: api/Customers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(Guid id)
    {
        var customer = await _uow.Customers.GettingCustomerByIdWithoutIncludesAsync(id);

        if (customer == null) return NotFound();

        return customer;
    }

    // PUT: api/Customers/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(Guid id, Customer customer)
    {
        if (id != customer.Id) return BadRequest();

        try
        {
            _uow.Customers.Update(customer);
            await _uow.SaveChangesAsync();
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
    public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
    {
        _uow.Customers.Add(customer);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetCustomer", new {id = customer.Id}, customer);
    }

    // DELETE: api/Customers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        var customer = await _uow.Customers.GettingCustomerByIdWithoutIncludesAsync(id);
        if (customer == null) return NotFound();

        _uow.Customers.Remove(customer);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private bool CustomerExists(Guid id)
    {
        return _uow.Customers.Exists(id);
    }
}