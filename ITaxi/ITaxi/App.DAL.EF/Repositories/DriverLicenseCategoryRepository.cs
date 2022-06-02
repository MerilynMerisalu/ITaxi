using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class DriverLicenseCategoryRepository: BaseEntityRepository<DriverLicenseCategory, AppDbContext>, IDriverLicenseCategoryRepository
{
    public DriverLicenseCategoryRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}