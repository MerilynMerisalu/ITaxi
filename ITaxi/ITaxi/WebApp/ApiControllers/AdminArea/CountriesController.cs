using App.Contracts.BLL;
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

        private readonly IAppBLL _appBLL;
        private readonly IMapper _mapper;

        public CountriesController( IMapper mapper, IAppBLL appBLL)
        {
            _mapper = mapper;
            _appBLL = appBLL;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<IEnumerable<App.BLL.DTO.AdminArea.CountryDTO>> GetCountries()
        {
          return (await _appBLL.Countries.GetAllCountriesOrderedByCountryNameAsync())
              .Select(e => _mapper.Map<App.BLL.DTO.AdminArea.CountryDTO>(e));
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<App.BLL.DTO.AdminArea.CountryDTO>> GetCountry(Guid id)
        {

            var country = await _appBLL.Countries.FirstOrDefaultAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return _mapper.Map<App.BLL.DTO.AdminArea.CountryDTO>(country);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(Guid id, App.BLL.DTO.AdminArea.CountryDTO countryDto)
        {
            if (id != countryDto.Id)
            {
                return BadRequest();
            }

            var country = await _appBLL.Countries.FirstOrDefaultAsync(id, noIncludes: true);
            

            try
            {
                if (country != null)
                {
                    country.CountryName = countryDto.CountryName;
                    country.UpdatedBy = User.GettingUserEmail();
                    country.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _appBLL.Countries.Update(country);
                    await _appBLL.SaveChangesAsync();
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
        public async Task<ActionResult<CountryDTO>> PostCountry(App.BLL.DTO.AdminArea.CountryDTO country)
        {
            country.CreatedBy = User.GettingUserEmail();
            country.CreatedAt = DateTime.Now.ToUniversalTime();
            country.UpdatedBy = User.GettingUserEmail();
            country.UpdatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.Countries.Add(country);
            await _appBLL.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(Guid id)
        {

            var country = await _appBLL.Countries.FirstOrDefaultAsync(id, noIncludes: true);
            if (country == null)
            {
                return NotFound();
            }

            await _appBLL.Countries.RemoveAsync(country.Id);
            await _appBLL.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(Guid id)
        {
            return _appBLL.Countries.Exists(id);
        }
    }
}
