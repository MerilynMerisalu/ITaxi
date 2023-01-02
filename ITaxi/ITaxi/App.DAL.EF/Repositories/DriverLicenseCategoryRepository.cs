using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class DriverLicenseCategoryRepository : BaseEntityRepository<DriverLicenseCategoryDTO, App.Domain.DriverLicenseCategory, AppDbContext>,
    IDriverLicenseCategoryRepository
{
    public DriverLicenseCategoryRepository(AppDbContext dbContext, IMapper<DriverLicenseCategoryDTO, DriverLicenseCategory> mapper) 
        : base(dbContext, mapper)
    {
    }


    public async Task<IEnumerable<DriverLicenseCategoryDTO>> GetAllDriverLicenseCategoriesOrderedAsync(
        bool noTracking = true)
    {
        return (await CreateQuery(noTracking)
            .OrderBy(d => d.DriverLicenseCategoryName).ToListAsync())
            .Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<DriverLicenseCategoryDTO> GetAllDriverLicenseCategoriesOrdered(bool noTracking = true)
    {
        return CreateQuery(noTracking)
            .OrderBy(c => c.DriverLicenseCategoryName).ToList()
            .Select(e => Mapper.Map(e))!;
    }
}