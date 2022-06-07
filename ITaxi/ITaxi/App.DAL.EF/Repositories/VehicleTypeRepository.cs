using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class VehicleTypeRepository: BaseEntityRepository<VehicleType, AppDbContext>, IVehicleTypeRepository
{
    public VehicleTypeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }


    public async Task<IEnumerable<VehicleType>> GetAllVehicleTypesOrderedAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).OrderBy(v => v.VehicleTypeName).ToListAsync();
    }

    public IEnumerable<VehicleType> GetAllVehicleTypesOrdered(bool noTracking = true)
    {
        return  CreateQuery(noTracking).OrderBy(v => v.VehicleTypeName).ToList();

    }
}