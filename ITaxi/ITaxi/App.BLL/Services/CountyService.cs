using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class CountyService: BaseEntityService<App.BLL.DTO.AdminArea.CountyDTO, DAL.DTO.AdminArea.CountyDTO, ICountyRepository>
, ICountyService
{
    public CountyService(ICountyRepository repository, IMapper<CountyDTO, DAL.DTO.AdminArea.CountyDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<CountyDTO>> GetAllCountiesOrderedByCountyNameAsync(bool noTracking = true)
    {
        return (await Repository.GetAllCountiesOrderedByCountyNameAsync(noTracking))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CountyDTO> GetAllCountiesOrderedByCountyName(bool noTracking = true)
    {
        return Repository.GetAllCountiesOrderedByCountyName(noTracking).Select(e => Mapper.Map(e))!;
    }

    public Task<bool> HasCities(Guid countyId)
    {
        throw new NotImplementedException();
    }
}