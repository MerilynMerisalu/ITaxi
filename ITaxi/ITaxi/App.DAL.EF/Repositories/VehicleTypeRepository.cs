using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class VehicleTypeRepository : BaseEntityRepository<VehicleType, AppDbContext>, IVehicleTypeRepository
{
    public VehicleTypeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }


    public async Task<IEnumerable<VehicleType>> GetAllVehicleTypesOrderedAsync(bool noTracking = true)
    {
#warning: special handling of OrderBy to account for language transalation
        var res = await CreateQuery(noTracking).ToListAsync();
        return res.OrderBy(x => (string) x.VehicleTypeName).ToList();
    }

    public IEnumerable<VehicleType> GetAllVehicleTypesOrdered(bool noTracking = true)
    {
#warning: special handling of OrderBy to account for language transalation
        return CreateQuery(noTracking)
            .ToList() // Bring into memory "Materialize"
            .OrderBy(v => v.VehicleTypeName).ToList();
    }

    protected override IQueryable<VehicleType> CreateQuery(bool noTracking = true)
    {
        return base.CreateQuery(noTracking).Include(t => t.VehicleTypeName)
            .ThenInclude(t => t.Translations);
    }
}