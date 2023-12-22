﻿using Base.Service;
using Base.Service.Contracts;
using Public.App.DTO.v1.AdminArea;
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
        public VehicleModelService(IHttpClientFactory clientProvider, IAppState appState) : 
            base(clientProvider.CreateClient("API"), appState)
        {
        }

        protected override string EndpointUri => "/adminArea/vehicleModels";

        public async Task<IEnumerable<VehicleModel?>> GetAllVehicleModelsAsync()
        {
            return (await base.GetAllAsync()).ToList();
        }
    }
}
