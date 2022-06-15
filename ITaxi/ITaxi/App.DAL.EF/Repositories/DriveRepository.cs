using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class DriveRepository: BaseEntityRepository<Drive, AppDbContext>, IDriveRepository
{
    public DriveRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}