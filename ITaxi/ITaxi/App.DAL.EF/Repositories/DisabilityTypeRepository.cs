using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class DisabilityTypeRepository: BaseEntityRepository<DisabilityType, AppDbContext>,
    IDisabilityTypeRepository
{
    public DisabilityTypeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<DisabilityType>> GetAllOrderedDisabilityTypesAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).OrderBy(d => d.DisabilityTypeName).ToListAsync();
    }

    public IEnumerable<DisabilityType> GetAllOrderedDisabilityTypes(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(d => d.DisabilityTypeName).ToList();

    }
}