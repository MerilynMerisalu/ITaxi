using Base.Service;
using Base.Service.Contracts;
using Public.App.DTO.v1.CustomerArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ITaxi.Service
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking?>> GetAllBookingsAsync();
        Task<Booking?> GetBookingByIdAsync(Guid id);
        Task RemoveBookingByIdAsync(Guid id);
        Task DeclineBookingById(Guid id);
    }

    public class BookingService : BaseEntityService<Booking, Guid>, IBookingService
    {
        public BookingService(IHttpClientFactory clientProvider, IAppState appState) : 
            base(clientProvider.CreateClient("API"), appState)
        {
        }
        protected virtual string DelineBookingEndPointUri => "Decline/";
        protected override string EndpointUri => "customerArea/bookings/";

        public async Task DeclineBookingById(Guid id)
        {
            await Client.GetAsync
                (Client.BaseAddress + EndpointUri + DelineBookingEndPointUri + id);
        }

        public async Task<IEnumerable<Booking?>> GetAllBookingsAsync()
        {
            return (await base.GetAllAsync()).ToList();
        }

        public async Task<Booking?> GetBookingByIdAsync(Guid id)
        {
            return await base.GetEntityByIdAsync(id);
        }

        public async Task RemoveBookingByIdAsync(Guid id)
        {
             await base.RemoveEntityAsync(id);
        }
    }
}
