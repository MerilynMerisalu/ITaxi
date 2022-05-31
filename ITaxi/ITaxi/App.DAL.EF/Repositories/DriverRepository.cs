using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class DriverRepository: BaseEntityRepository<Driver, AppDbContext>, IDriverRepository
{
    public DriverRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}