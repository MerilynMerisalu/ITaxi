using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class CityRepository: BaseEntityRepository<City, AppDbContext>, ICityRepository
{
    public CityRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}