using Base.Service;
using Base.Service.Contracts;
using Public.App.DTO.v1.CustomerArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITaxi.Service
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking?>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(Guid id);
    }

    public class BookingService : BaseEntityService<Booking, Guid>, IBookingService
    {
        public BookingService(IHttpClientFactory clientProvider, IAppState appState) : 
            base(clientProvider.CreateClient("API"), appState)
        {
        }

        protected override string EndpointUri => "/customerArea/bookings/";

        public async Task<IEnumerable<Booking?>> GetAllBookingsAsync()
        {
            return (await base.GetAllAsync()).ToList();
        }

        public async Task<Booking?> GetBookingByIdAsync(Guid id)
        {
            return await base.GetEntityByIdAsync(id);
        }
    }
}
