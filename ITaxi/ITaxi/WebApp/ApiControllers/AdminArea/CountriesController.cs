using App.Contracts.DAL;
using App.DAL.DTO.AdminArea;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Base.Extensions;

namespace WebApp.ApiControllers.AdminArea
{
    [Route("api/adminArea/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        
        private readonly IAppUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CountriesController( IMapper mapper, IAppUnitOfWork uow)
        {
            
            _mapper = mapper;
            _uow = uow;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<IEnumerable<CountryDTO>> GetCountries()
        {
          return (await _uow.Countries.GetAllCountriesOrderedByCountryNameAsync()).Select(e => _mapper.Map<CountryDTO>(e));
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDTO>> GetCountry(Guid id)
        {

            var country = await _uow.Countries.FirstOrDefaultAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(Guid id, CountryDTO countryDto)
        {
            if (id != countryDto.Id)
            {
                return BadRequest();
            }

            var country = await _uow.Countries.FirstOrDefaultAsync(id, noIncludes: true);
            

            try
            {
                if (country != null)
                {
                    country.CountryName = countryDto.CountryName;
                    country.UpdatedBy = User.GettingUserEmail();
                    country.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _uow.Countries.Update(country);
                    await _uow.SaveChangesAsync();
                }

                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CountryDTO>> PostCountry(CountryDTO country)
        {
            country.CreatedBy = User.GettingUserEmail();
            country.CreatedAt = DateTime.Now.ToUniversalTime();
            country.UpdatedBy = User.GettingUserEmail();
            country.UpdatedAt = DateTime.Now.ToUniversalTime();
            _uow.Countries.Add(country);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(Guid id)
        {

            var country = await _uow.Countries.FirstOrDefaultAsync(id, noIncludes: true);
            if (country == null)
            {
                return NotFound();
            }

            await _uow.Countries.RemoveAsync(country.Id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(Guid id)
        {
            return _uow.Countries.Exists(id);
        }
    }
}
