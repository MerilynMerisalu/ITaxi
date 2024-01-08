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
    public interface IDriverLicenseCategoryService
    {
        Task<IEnumerable<DriverLicenseCategory?>> GetAllDriverLicenseCategoriesAsync();
    }

    public class DriverLicenseCategoryService : BaseEntityService<DriverLicenseCategory, Guid>, IDriverLicenseCategoryService
    {
        public DriverLicenseCategoryService(IHttpClientFactory clientProvider, IAppState appState) : 
            base(clientProvider.CreateClient("API"), appState)
        {
        }

        protected override string EndpointUri => "adminarea/driverlicenseCategories/";

        public async Task<IEnumerable<DriverLicenseCategory?>> GetAllDriverLicenseCategoriesAsync()
        {
            return await base.GetAllAsync();
        }
    }
}
