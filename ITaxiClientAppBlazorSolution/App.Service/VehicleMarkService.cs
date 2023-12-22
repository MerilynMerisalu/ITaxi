using Base.Service;
using Base.Service.Contracts;
using Public.App.DTO.v1.AdminArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITaxi.Service
{
    public interface IVehicleMark
    {
        Task<IEnumerable<VehicleMark?>> GetAllVehicleMarksAsync();
    }
    public class VehicleMarkService : BaseEntityService<VehicleMark, Guid>, IVehicleMark
    {
        public VehicleMarkService(IHttpClientFactory clientProvider, IAppState appState) : 
            base(clientProvider.CreateClient("API"), appState)
        {
        }

        protected override string EndpointUri => "/adminArea/vehicleMarks/";

        public async Task<IEnumerable<VehicleMark?>> GetAllVehicleMarksAsync()
        {
            return (await base.GetAllAsync()).ToList();
        }
    }
}
