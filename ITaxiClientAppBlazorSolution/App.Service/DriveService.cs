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
    public interface IDriveService
    {
        Task<IEnumerable<Drive?>> GetAllDrivesAsync();
        Task<Drive?> GetDriveByIdAsync(Guid id);

    }
    public class DriveService : BaseEntityService<Drive, Guid>, IDriveService
    {
        public DriveService(IHttpClientFactory clientProvider, IAppState appState) : 
            base(clientProvider.CreateClient("API"), appState)
        {
        }

        protected override string EndpointUri => "/driverarea/drives/";

        public async Task<IEnumerable<Drive?>> GetAllDrivesAsync()
        {
            return (await base.GetAllAsync()).ToList();
        }

        public async Task<Drive?> GetDriveByIdAsync(Guid id)
        {
            return await base.GetEntityByIdAsync(id);
        }
    }
}
