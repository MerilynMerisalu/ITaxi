using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class DriverAndDriverLicenseCategoryRepository :
    BaseEntityRepository<DriverAndDriverLicenseCategoryDTO, App.Domain.DriverAndDriverLicenseCategory, AppDbContext>,
    IDriverAndDriverLicenseCategoryRepository

{
    public DriverAndDriverLicenseCategoryRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO,
    App.Domain.DriverAndDriverLicenseCategory> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<string?> GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(Guid id, string separator = ", ")
    {
        var driverLicenseCategoryNames = await CreateQuery()
            .Where(i => i.DriverId.Equals(id))
            .OrderBy(c => c.DriverLicenseCategory!.DriverLicenseCategoryName)
            .Select(dl => dl.DriverLicenseCategory!.DriverLicenseCategoryName)
            .ToListAsync();
        return string.Join(separator, driverLicenseCategoryNames);
    }

    public string GetAllDriverLicenseCategoriesBelongingToTheDriver(Guid id, string separator = ", ")
    {
        var driverLicenseCategoryNamesAsList = CreateQuery()
            .Where(i => i.DriverId.Equals(id))
            .OrderBy(c => c.DriverLicenseCategory!.DriverLicenseCategoryName)
            .Select(dl => dl.DriverLicenseCategory!.DriverLicenseCategoryName)
            .ToList();
        return string.Join(separator, driverLicenseCategoryNamesAsList);
    }

    public async Task<List<DriverAndDriverLicenseCategoryDTO?>>
        RemovingAllDriverAndDriverLicenseEntitiesByDriverIdAsync(Guid id, bool noTracking = true)
    {
        var driverAndDriverLicenseCategories =
            await CreateQuery(noTracking, true)
                .Where(dl => dl.DriverId.Equals(id))
                .Select(dl => dl).ToListAsync();


        return RemoveAll(driverAndDriverLicenseCategories.Select(e => Mapper.Map(e!)).ToList()!)!;
        
    }

    public async Task<bool> HasAnyDriversAsync(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return await CreateQuery().AnyAsync(dlc => dlc.DriverLicenseCategoryId.Equals(id));
    }

    public bool HasAnyDrivers(Guid id, Guid? userId = null, string? roleName = null, bool noTracking = true)
    {
        return CreateQuery().Any(dlc => dlc.DriverLicenseCategoryId.Equals(id));
    }


    protected override IQueryable<DriverAndDriverLicenseCategory> CreateQuery(bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        var query = base.CreateQuery(noTracking, noIncludes, showDeleted);
        if (noTracking) query = query.AsNoTracking();
        if (!noIncludes)
            query = query.Include(c => c.DriverLicenseCategory)
                         .Include(c => c.Driver);
        return query;
    }
}