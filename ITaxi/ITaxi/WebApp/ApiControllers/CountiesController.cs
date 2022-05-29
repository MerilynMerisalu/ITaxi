#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountiesController : ControllerBase
    {
        private readonly IAppUnitOfWork _uow;

        public CountiesController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/Counties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<County>>> GetCounties()
        {
            return Ok(await _uow.Counties.GetAllAsync());
        }

        // GET: api/Counties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<County>> GetCounty(Guid id)
        {
            var county = await _uow.Counties.FirstOrDefaultAsync(id);

            if (county == null)
            {
                return NotFound();
            }

            return county;
        }

        // PUT: api/Counties/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCounty(Guid id, County county)
        {
            if (id != county.Id)
            {
                return BadRequest();
            }
            _uow.Counties.Update(county);
            await _uow.SaveChangesAsync();
                

            return NoContent();
        }

        // POST: api/Counties
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<County>> PostCounty(County county)
        {
            _uow.Counties.Add(county);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetCounty", new { id = county.Id }, county);
        }

        // DELETE: api/Counties/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCounty(Guid id)
        {
            var county = await _uow.Counties.FirstOrDefaultAsync(id);
            if (county == null)
            {
                return NotFound();
            }

            _uow.Counties.Remove(county);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private bool CountyExists(Guid id)
        {
            return _uow.Counties.Exists(id);
        }
    }
}
