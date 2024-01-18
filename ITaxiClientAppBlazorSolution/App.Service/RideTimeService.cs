using Base.Service;
using Base.Service.Contracts;
using Public.App.DTO.v1.DriverArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITaxi.Service
{
    public interface IRideTime
    {
        Task<IEnumerable<RideTime?>> GetAllRideTimesAsync();
        Task<RideTime?> GetRideTimeByIdAsync(Guid id);
        Task<List<RideTime?>> AddRideTimesAsync(List<RideTime?>? rideTimes);
        Task DeleteRideTimeByIdAsync(Guid id);
    }

    public class RideTimeService : BaseEntityService<RideTime, Guid>, IRideTime
    {
        public RideTimeService(IHttpClientFactory clientProvider, IAppState appState) : 
            base(clientProvider.CreateClient("API"), appState)
        {
        }

        protected override string EndpointUri => "/driverarea/ridetimes/";

        public async Task<List<RideTime?>> AddRideTimesAsync(List<RideTime?>? rideTimes)
        {
            return (await base.AddEntitiesAsync(rideTimes)).ToList();
        }

        public async Task DeleteRideTimeByIdAsync(Guid id)
        {
             await base.RemoveEntityAsync(id);
        }

        public async Task<IEnumerable<RideTime?>> GetAllRideTimesAsync()
        {
            return (await base.GetAllAsync()).ToList();
        }

        public async Task<RideTime?> GetRideTimeByIdAsync(Guid id)
        {
            return (await base.GetEntityByIdAsync(id));
        }
    }
}
