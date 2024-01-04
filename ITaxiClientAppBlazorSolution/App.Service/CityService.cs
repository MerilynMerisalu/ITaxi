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
    public interface ICityService
    {
        Task<IEnumerable<City?>>GetAllCitiesAsync();
    }
    public class CityService : BaseEntityService<City, Guid>, ICityService
    {
        public CityService(IHttpClientFactory clientProvider, IAppState appState) : 
            base(clientProvider.CreateClient("API"), appState)
        {
        }

        protected override string EndpointUri => "adminarea/cities/";

        public async Task<IEnumerable<City?>> GetAllCitiesAsync()
        {
            return await base.GetAllAsync();
        }
    }
}
