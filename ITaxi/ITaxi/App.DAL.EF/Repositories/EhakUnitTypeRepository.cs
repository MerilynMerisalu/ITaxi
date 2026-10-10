using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.Mappers;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class EhakUnitTypeRepository: BaseEntityRepository<App.DAL.DTO.AdminArea.EhakUnitTypeDTO, 
    App.Domain.EhakUnitType, AppDbContext>, IEhakUnitTypeRepository
{
    public EhakUnitTypeRepository(AppDbContext dbContext, 
        IMapper<EhakUnitTypeDTO, EhakUnitType> mapper) : base(dbContext, mapper)
    {
    }
}