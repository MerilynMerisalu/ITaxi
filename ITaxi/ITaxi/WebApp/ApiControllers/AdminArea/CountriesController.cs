using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.DAL.DTO.AdminArea;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.DAL.EF.Repositories;
using App.Domain;
using AutoMapper;
using Base.Extensions;

namespace WebApp.ApiControllers.AdminArea
{
    [Route("api/adminArea/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly CountryRepository _repo;
        private readonly IMapper _mapper;

        public CountriesController(AppDbContext context, CountryRepository repo, IMapper mapper)
        {
            _context = context;
            _repo = repo;
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<IEnumerable<CountryDTO>> GetCountries()
        {
          return (await _repo.GetAllCountriesOrderedByCountryNameAsync()).Select(e => _mapper.Map<CountryDTO>(e));
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDTO>> GetCountry(Guid id)
        {

            var country = await _repo.FirstOrDefaultAsync(id);

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

            var country = await _repo.FirstOrDefaultAsync(id, noIncludes: true);
            

            try
            {
                if (country != null)
                {
                    country.CountryName = countryDto.CountryName;
                    country.UpdatedBy = User.GettingUserEmail();
                    country.UpdatedAt = DateTime.Now.ToUniversalTime();
                    _context.Countries.Add((_mapper.Map<Country>(country)));
                    await _context.SaveChangesAsync();
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
            _context.Countries.Add(_mapper.Map<Country>(country));
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(Guid id)
        {

            var country = await _repo.FirstOrDefaultAsync(id, noIncludes: true);
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(_mapper.Map<Country>(country));
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountryExists(Guid id)
        {
            return _repo.Exists(id);
        }
    }
}
