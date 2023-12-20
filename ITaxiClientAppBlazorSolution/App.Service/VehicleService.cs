using ITaxi.Public.DTO.v1.DriverArea;
using Base.Service;
using System.Net.Http;
using Base.Service.Contracts;
using System.Net.Http.Json;

namespace ITaxi.Service
{
    
    public interface IVehicleService
    {
        Task<IEnumerable<Vehicle?>> GetAllVehiclesAsync();
        Task<Vehicle?> GetVehicleByIdAsync(Guid id);
        Task<Vehicle> AddVehicle(Vehicle vehicle);
        Task UpdateVehicle(Guid Id,Vehicle vehicle);
        Task DeleteVehicleByIdAsync(Guid id);
        Task<IEnumerable<int>> GetManufactureYears();
    }
    public class VehicleService : BaseEntityService<Vehicle, Guid>, IVehicleService
    {
        public VehicleService(IHttpClientFactory clientProvider, IAppState appState) : 
            base(clientProvider.CreateClient("API"), appState)
        {
        }
        protected virtual string ManufactureYearsEndPointUri => "GetManufactureYears";
        protected override string EndpointUri => "driverarea/vehicles/";

        public async Task<Vehicle> AddVehicle(Vehicle vehicle)
        {
            return await base.AddEntity(vehicle);
        }

        public async Task DeleteVehicleByIdAsync(Guid id)
        {
            await base.RemoveEntityAsync(id);
        }

        public async Task<IEnumerable<Vehicle?>> GetAllVehiclesAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<IEnumerable<int>> GetManufactureYears()
        {
            return (await Client.GetFromJsonAsync<IEnumerable<int>>
                (Client.BaseAddress + EndpointUri + ManufactureYearsEndPointUri)).ToList();
        }

        public async Task<Vehicle?> GetVehicleByIdAsync(Guid id)
        {
            return await base.GetEntityByIdAsync(id);
        }

        public async Task UpdateVehicle(Guid id,Vehicle vehicle)
        {
             await base.UpdateEntityAsync(id,vehicle);
        }
    }
}
