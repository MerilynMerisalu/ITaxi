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

    public async Task<IEnumerable<CountyDTO>> GetAllCountiesOrderedByCountyNameAsync(bool noTracking = true, bool noIncludes = false)
    {
        return (await Repository.GetAllCountiesOrderedByCountyNameAsync(noTracking, noIncludes))
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<CountyDTO> GetAllCountiesOrderedByCountyName(bool noTracking = true, bool noIncludes = false)
    {
        return Repository.GetAllCountiesOrderedByCountyName(noTracking, noIncludes).Select(e => Mapper.Map(e))!;
    }

    /*public async Task<IEnumerable<CountyDTO>> GetAllCountiesOrderedByCountryNameAsync(bool noTracking = true, bool noIncludes = false)
    {
        return (await Repository.GetAllCountiesOrderedByCountryNameAsync(noTracking, noIncludes))
            .Select(e => Mapper.Map(e))!;
    }
    */

    /*public IEnumerable<CountyDTO> GetAllCountiesOrderedByCountryName(bool noTracking = true, bool noIncludes = false)
    {
        return Repository.GetAllCountiesOrderedByCountryName(noTracking, noIncludes).Select(e => Mapper.Map(e))!;
    }*/

    public Task<bool> HasCities(Guid countyId)
    {
        throw new NotImplementedException();
    }
}