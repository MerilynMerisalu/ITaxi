using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class DriverAndDriverLicenseCategoryRepository: BaseEntityRepository<DriverAndDriverLicenseCategory, AppDbContext>,
    IDriverAndDriverLicenseCategoryRepository

{
    public DriverAndDriverLicenseCategoryRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}