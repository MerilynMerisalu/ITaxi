using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class RideTimeRepository: BaseEntityRepository<RideTime, AppDbContext>, IRideTimeRepository
{
    public RideTimeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}