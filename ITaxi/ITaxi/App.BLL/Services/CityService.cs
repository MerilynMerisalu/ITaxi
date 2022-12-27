using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class CityService: BaseEntityService<App.BLL.DTO.AdminArea.CityDTO, DAL.DTO.AdminArea.CityDTO, ICityRepository>
{
    public CityService(ICityRepository repository, IMapper<CityDTO, DAL.DTO.AdminArea.CityDTO> mapper) : base(repository, mapper)
    {
    }
}