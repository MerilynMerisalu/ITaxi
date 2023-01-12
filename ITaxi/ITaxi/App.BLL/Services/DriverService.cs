using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.EF.Repositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class DriverService: BaseEntityService<App.BLL.DTO.AdminArea.DriverDTO, App.DAL.DTO.AdminArea.DriverDTO
, IDriverRepository>, IDriverService
{
    public DriverService(IDriverRepository repository, IMapper<DriverDTO, DAL.DTO.AdminArea.DriverDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<DriverDTO>> GetAllDriversOrderedByLastNameAsync(bool noTracking = true)
    {
        return (await Repository.GetAllDriversOrderedByLastNameAsync(noTracking))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<DriverDTO> GetAllDriversOrderedByLastName(bool noTracking = true)
    {
        return Repository.GetAllDriversOrderedByLastName(noTracking).Select(e => Mapper.Map(e))!;
    }

    public async Task<DriverDTO> GettingDriverByVehicleAsync(Guid driverAppUserId, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingDriverByVehicleAsync(driverAppUserId, noTracking))!;
    }

    public async Task<DriverDTO> GettingDriverByAppUserIdAsync(Guid userId)
    {
        return Mapper.Map(await Repository.SingleOrDefaultAsync(d => d!.AppUserId.Equals(userId)))!;
    }
}