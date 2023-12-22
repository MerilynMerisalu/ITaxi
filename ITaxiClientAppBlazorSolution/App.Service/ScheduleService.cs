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
    public interface IScheduleService
    {
        Task<IEnumerable<Schedule?>> GetAllSchedulesAsync();
        Task<Schedule?> GetScheduleByIdAsync(Guid id);
        Task<Schedule> AddSchedule(Schedule schedule);
        Task DeleteScheduleByIdAsync(Guid id);
    }
    public class ScheduleService : BaseEntityService<Schedule, Guid>, IScheduleService
    {
        public ScheduleService(IHttpClientFactory clientProvider, IAppState appState) : 
            base(clientProvider.CreateClient("API"), appState)
        {
        }

        protected override string EndpointUri => "driverarea/schedules/";

        public async Task<Schedule> AddSchedule(Schedule schedule)
        {
            return await base.AddEntity(schedule);
        }

        public async Task DeleteScheduleByIdAsync(Guid id)
        {
            await base.RemoveEntityAsync(id);
        }

        public async Task<IEnumerable<Schedule?>> GetAllSchedulesAsync()
        {
            return await base.GetAllAsync();
        }

        public async Task<Schedule?> GetScheduleByIdAsync(Guid id)
        {
            return await base.GetEntityByIdAsync(id);
        }
    }
}
