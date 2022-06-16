#nullable enable
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
    public class BookingsController : ControllerBase
    {
        private readonly IAppUnitOfWork _uow;

        public BookingsController(IAppUnitOfWork uow)
        {
            _uow = uow;
        }

        // GET: api/Bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return Ok(await _uow.Bookings.GettingAllOrderedBookingsWithoutIncludesAsync()) ;
        }

        // GET: api/Bookings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(Guid id)
        {
            var booking = await _uow.Bookings.GettingBookingWithoutIncludesByIdAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // PUT: api/Bookings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBooking(Guid id, Booking booking)
        {
            if (id != booking.Id)
            {
                return BadRequest();
            }

           

            try
            {
                _uow.Bookings.Update(booking);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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

        // POST: api/Bookings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Booking>> PostBooking(Booking booking)
        {
            _uow.Bookings.Add(booking);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
        }

        // DELETE: api/Bookings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var booking = await _uow.Bookings.GettingBookingWithoutIncludesByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _uow.Bookings.Remove(booking);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private bool BookingExists(Guid id)
        {
            return _uow.Bookings.Exists(id);
        }

       
    }
}
