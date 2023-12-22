using Base.Service;
using Base.Service.Contracts;
using Public.App.DTO.v1.AdminArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ITaxi.Service
{
    public interface IVehicleType
    {
        Task<IEnumerable<VehicleType?>> GetAllVehicleTypesAsync();
    }
    public class VehicleTypeService : BaseEntityService<VehicleType, Guid>, IVehicleType
    {
        public VehicleTypeService(IHttpClientFactory ClientProvider, IAppState appState) : 
            base(ClientProvider.CreateClient("API"), appState)
        {

        }
        

        protected override string EndpointUri => "/adminarea/vehicletypes/";

        public async Task<IEnumerable<VehicleType?>> GetAllVehicleTypesAsync()
        {
            return (await base.GetAllAsync()).ToList();
        }
    }
}
