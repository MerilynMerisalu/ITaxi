using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class DisabilityTypeService: BaseEntityService<App.BLL.DTO.AdminArea.DisabilityTypeDTO,
    App.DAL.DTO.AdminArea.DisabilityTypeDTO, IDisabilityTypeRepository>, IDisabilityTypeService
{
    public DisabilityTypeService(IDisabilityTypeRepository repository, IMapper<DisabilityTypeDTO, DAL.DTO.AdminArea.DisabilityTypeDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<DisabilityTypeDTO>> GetAllDisabilityTypeDtoAsync(string? roleName = null, bool noTracking = true)
    {
        return (await Repository.GetAllDisabilityTypeDtoAsync(roleName, noTracking)).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<DisabilityTypeDTO> GetAllDisabilityTypeDto(string? roleName = null, bool noTracking = true)
    {
        return Repository.GetAllDisabilityTypeDto(roleName, noTracking).Select(e => Mapper.Map(e))!;
    }

    public async Task<IEnumerable<DisabilityTypeDTO>> GetAllOrderedDisabilityTypesAsync(bool noTracking = true)
    {
        return (await Repository.GetAllOrderedDisabilityTypesAsync(noTracking)).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<DisabilityTypeDTO> GetAllOrderedDisabilityTypes(bool noTracking = true)
    {
        return Repository.GetAllOrderedDisabilityTypes(noTracking).Select(e => Mapper.Map(e))!;
    }
}