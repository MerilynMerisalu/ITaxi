using ITaxi.Public.DTO.v1.DriverArea;
using Base.Service;
using System.Net.Http;
using Base.Service.Contracts;

namespace ITaxi.Service
{
    
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle?>> GetAllVehiclesAsync();
        Task<Vehicle?> GetVehicleByIdAsync(Guid id);
        Task<Vehicle?> DeleteVehicleByIdAsync(Guid id);
    }
    public class VehicleService : BaseEntityService<Vehicle, Guid>, IVehicleService
    {
        public VehicleService(IHttpClientFactory clientProvider, IAppState appState) : base(clientProvider.CreateClient("API"), appState)
        {
        }

        protected override string EndpointUri => "/driverarea/vehicles/";

        public async Task<Vehicle?> DeleteVehicleByIdAsync(Guid id)
        {
            return await base.RemoveEntityAsync(id);
        }

        public async Task<IEnumerable<Vehicle?>> GetAllVehiclesAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(Guid id)
        {
            return await base.GetEntityByIdAsync(id);
        }
    }
}
