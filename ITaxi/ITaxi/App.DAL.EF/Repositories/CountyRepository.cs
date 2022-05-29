using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class CountyRepository: BaseEntityRepository<County, AppDbContext>, ICountyRepository
{
    public CountyRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}