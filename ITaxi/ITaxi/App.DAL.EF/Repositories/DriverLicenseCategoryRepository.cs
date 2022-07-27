using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class DriverLicenseCategoryRepository : BaseEntityRepository<DriverLicenseCategory, AppDbContext>,
    IDriverLicenseCategoryRepository
{
    public DriverLicenseCategoryRepository(AppDbContext dbContext) : base(dbContext)
    {
    }


    public async Task<IEnumerable<DriverLicenseCategory>> GetAllDriverLicenseCategoriesOrderedAsync(
        bool noTracking = true)
    {
        return await CreateQuery(noTracking).OrderBy(d => d.DriverLicenseCategoryName).ToListAsync();
    }

    public IEnumerable<DriverLicenseCategory> GetAllDriverLicenseCategoriesOrdered(bool noTracking = true)
    {
        return CreateQuery(noTracking)
            .OrderBy(c => c.DriverLicenseCategoryName).ToList();
    }
}