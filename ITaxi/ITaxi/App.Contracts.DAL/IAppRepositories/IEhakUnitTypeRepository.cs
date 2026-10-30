using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IEhakUnitTypeRepository: IEntityRepository<App.DAL.DTO.AdminArea.EhakUnitTypeDTO>, 
    IEhakUnitTypeCustomRepository<App.DAL.DTO.AdminArea.EhakUnitTypeDTO>
{
}

public interface IEhakUnitTypeCustomRepository<TEntity>
{
   Task<bool>DoesEhakUnitTypeExistByNameAsync(string name);
}