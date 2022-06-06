using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class DriverAndDriverLicenseCategoryRepository: BaseEntityRepository<DriverAndDriverLicenseCategory, AppDbContext>,
    IDriverAndDriverLicenseCategoryRepository

{
    public DriverAndDriverLicenseCategoryRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    protected override IQueryable<DriverAndDriverLicenseCategory> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query.AsNoTracking();
        }

        query = query.Include(c => c.DriverLicenseCategory)
            .Include(c => c.Driver);
        return query;
    }

    public async Task<IEnumerable<string>> GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(Guid id)
    {
        var driverLicenseCategoryNames = await CreateQuery()
            .Where(i => i.DriverId.Equals(id))
            .OrderBy(c => c.DriverLicenseCategory!.DriverLicenseCategoryName)
            .Select(dl => dl.DriverLicenseCategory!.DriverLicenseCategoryName)
            .ToListAsync();
        return driverLicenseCategoryNames;
    }

    public string GetAllDriverLicenseCategoriesBelongingToTheDriver(Guid id, string separator = " ")
    {
        var driverLicenseCategoryNamesAsList =  CreateQuery()
            .Where(i => i.DriverId.Equals(id))
            .OrderBy(c => c.DriverLicenseCategory!.DriverLicenseCategoryName)
            .Select(dl => dl.DriverLicenseCategory!.DriverLicenseCategoryName)
            .ToList();

        return string.Join(separator, driverLicenseCategoryNamesAsList);
    }

    public async Task<List<DriverAndDriverLicenseCategory?>> RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(Guid id)
    {
        var driverAndDriverLicenseCategories =
            await CreateQuery()
                .Where(dl => dl.DriverId.Equals(id))
                .Select(dl => dl).ToListAsync();

        return RemoveAll(driverAndDriverLicenseCategories)!;
    }
}