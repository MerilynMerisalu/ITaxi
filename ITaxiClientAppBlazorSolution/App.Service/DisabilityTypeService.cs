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
    public interface IDisabilityTypeService
    {
        Task<List<DisabilityType?>?> GetAllDisabilityTypesAsync();
    }

    public class DisabilityTypeService : BaseEntityService<DisabilityType, Guid>, IDisabilityTypeService
    {
        public DisabilityTypeService(IHttpClientFactory clientProvider, IAppState appState) :
            base(clientProvider.CreateClient("API"), appState)
        {
        }

        protected override string EndpointUri => "adminarea/disabilityTypes/";

        public async Task<List<DisabilityType?>?> GetAllDisabilityTypesAsync()
        {
            return (await base.GetAllAsync()).ToList();
        }
    }
}
