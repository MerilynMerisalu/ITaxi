using Base.Service;
using Base.Service.Contracts;
using ITaxi.Public.DTO.v1.AdminArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITaxi.Service
{
    public interface IVehicleModel
    {
        Task<IEnumerable<VehicleModel?>> GetAllVehicleModelsAsync();
    }
    public class VehicleModelService : BaseEntityService<VehicleModel, Guid>, IVehicleModel
    {
        public VehicleModelService(HttpClient client, IAppState appState) : base(client, appState)
        {
        }

        protected override string EndpointUri => "/adminArea/vehicleModels";

        public async Task<IEnumerable<VehicleModel?>> GetAllVehicleModelsAsync()
        {
            return (await base.GetAllAsync()).ToList();
        }
    }
}
