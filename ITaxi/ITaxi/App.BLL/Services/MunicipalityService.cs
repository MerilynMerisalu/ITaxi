using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts.Mappers;

namespace App.BLL.Services;

public class MunicipalityService : BaseEntityService<App.BLL.DTO.AdminArea.MunicipalityDTO, DAL.DTO.AdminArea.MunicipalityDTO,
    IMunicipalityRepository>, IMuncipalityService
{
    public MunicipalityService(IMunicipalityRepository repository, IMapper<MunicipalityDTO, DAL.DTO.AdminArea.MunicipalityDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<MunicipalityDTO>> GetAllMunicipalitiesWithoutCountyAsync()
    {
        return (await Repository.GetAllMunicipalitiesWithoutCountyAsync()).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<MunicipalityDTO>> GetAllOrderedMunicipalitiesWithoutCountyAsync()
    {
        return (await Repository.GetAllOrderedMunicipalitiesWithoutCountyAsync()).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<MunicipalityDTO>> GetAllOrderedMunipalitiesAsync(bool showDeleted = false, bool showIgnored = false)
    {
        return (await Repository.GetAllOrderedMunipalitiesAsync(showIgnored: showIgnored, showDeleted: showDeleted)).Select(e => Mapper.Map(e))!;
    }

    public async Task<MunicipalityDTO?> FirstOrDefaultMunicipalityWithoutCountyAsync(Guid id, bool showIgnored = false)
    {
        return Mapper.Map(await Repository.FirstOrDefaultMunicipalityWithoutCountyAsync(id, showIgnored: showIgnored));
    }

    public IEnumerable<MunicipalityDTO> GetAllOrderedMunicipalitiesWithoutCounty()
    {
        return Repository.GetAllOrderedMunicipalitiesWithoutCounty().Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<MunicipalityDTO> GetAllOrderedMunicipalities(bool noTracking = true)
    {
        return Repository.GetAllOrderedMunicipalities(noTracking).Select(e => Mapper.Map(e))!;
    }

    public async Task<bool> HasAnyMunicipalitiesAsync(Guid id, bool noTracking = true)
    {
        return await Repository.HasAnyMunicipalitiesAsync(id, noTracking);
    }

    public bool HasAnyMunicipalities(Guid id, bool noTracking = true)
    {
        return Repository.HasAnyMunicipalities(id, noTracking);
    }

    public async Task<MunicipalityDTO?> GetMunicipalityByIdAsync(Guid id, bool noTracking = false, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        return Mapper.Map(await Repository.GetMunicipalityByIdAsync(id: id, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted, showIgnored: showIgnored));
    }

    public async Task<List<MunicipalityDTO?>> GetMunicipalitiesByCountyIdAsync(Guid countyId, bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        return (await Repository.GetMunicipalitiesByCountyIdAsync(countyId: countyId, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted, showIgnored: showIgnored))
            .Select(m => Mapper.Map(m)).ToList();
            
    }

    public List<MunicipalityDTO?> GetMunicipalitiesByCountyId(Guid countyId, bool noTracking = true, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
    {
        return ( Repository.GetMunicipalitiesByCountyId(countyId: countyId, noTracking: noTracking, noIncludes: noIncludes, showDeleted: showDeleted, showIgnored: showIgnored)
            .Select(m => Mapper.Map(m)).ToList());
    }
}