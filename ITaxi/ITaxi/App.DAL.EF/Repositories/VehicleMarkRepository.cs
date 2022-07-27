using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class VehicleMarkRepository : BaseEntityRepository<VehicleMark, AppDbContext>, IVehicleMarkRepository
{
    public VehicleMarkRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<VehicleMark>> GetAllVehicleMarkOrderedAsync(bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).OrderBy(v => v.VehicleMarkName).ToListAsync();
    }

    public IEnumerable<VehicleMark> GetAllVehicleMarkOrdered(bool noTracking = true)
    {
        return base.CreateQuery(noTracking).OrderBy(v => v.VehicleMarkName).ToList();
    }
}