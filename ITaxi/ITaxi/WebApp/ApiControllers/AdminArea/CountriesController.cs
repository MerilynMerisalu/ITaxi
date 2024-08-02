using App.Contracts.BLL;
using App.Contracts.DAL;
using App.Public.DTO.v1.AdminArea;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.ApiControllers.AdminArea
{
    [Route("api/v{version:apiVersion}/AdminArea/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CountriesController : ControllerBase
    {

        private readonly IAppBLL _appBLL;
        private readonly IMapper _mapper;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController( IMapper mapper, IAppBLL appBLL, ILogger<CountriesController> logger)
        {
            _mapper = mapper;
            _appBLL = appBLL;
            _logger = logger;
        }

        // GET: api/Countries
        [HttpGet]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType( typeof( IEnumerable<Country>), StatusCodes.Status200OK )] 
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IEnumerable<Country>>GetCountries()
        {
          var res = await _appBLL.Countries.GetAllCountriesOrderedByCountryNameAsync();
          return res.Select(c => _mapper.Map<Country>(c));
        }

        // GET: api/Countries/5
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Country), StatusCodes.Status200OK )] 
        [ProducesResponseType( StatusCodes.Status404NotFound )] 
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Country>> GetCountry(Guid id)
        {

            var country = await _appBLL.Countries.FirstOrDefaultAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return _mapper.Map<Country>(country);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:guid}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutCountry(Guid id, Country countryDto)
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
        [Produces("application/json")]
        [ProducesResponseType(typeof(Country), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Country>> PostCountry(Country country)
        {
            country.Id = Guid.NewGuid();
            country.CreatedBy = User.GettingUserEmail();
            country.CreatedAt = DateTime.Now.ToUniversalTime();
            country.UpdatedBy = User.GettingUserEmail();
            country.UpdatedAt = DateTime.Now.ToUniversalTime();
            _appBLL.Countries.Add(_mapper.Map<App.BLL.DTO.AdminArea.CountryDTO>(country));
            await _appBLL.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(Guid id)
        {

            var country = await _appBLL.Countries.FirstOrDefaultAsync(id, noIncludes: true, noTracking:true);
            if (country == null)
            {
                return NotFound();
            }
            if (await _appBLL.Countries.HasAnyCountiesAsync(id))
                return Content("Entity cannot be deleted because it has dependent entities!");


            await _appBLL.Countries.RemoveAsync(country.Id);
            await _appBLL.SaveChangesAsync();

            return NoContent();
        }
        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private bool CountryExists(Guid id)
        {
            return _appBLL.Countries.Exists(id);
        }
    }
}
