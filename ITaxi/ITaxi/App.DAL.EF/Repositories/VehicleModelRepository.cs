/*using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class VehicleModelRepository : BaseEntityRepository<VehicleModel, AppDbContext>, IVehicleModelRepository
{
    public VehicleModelRepository(AppDbContext dbContext) : base(dbContext)
    {
    }


    public async Task<IEnumerable<VehicleModel>> GetAllVehicleModelsWithoutVehicleMarksAsync(bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).ToListAsync();
    }

    public IEnumerable<VehicleModel> GetAllVehicleModelsWithoutVehicleMarks(bool noTracking = true)
    {
        return base.CreateQuery(noTracking).ToList();
    }

    public async Task<VehicleModel?> FirstOrDefaultVehicleModelWithoutVehicleMarkAsync(Guid id, bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).FirstOrDefaultAsync(v => v.Id.Equals(id));
    }

    public VehicleModel? FirstOrDefaultVehicleModelWithoutVehicleMark(Guid id, bool noTracking = true)
    {
        return base.CreateQuery(noTracking).FirstOrDefault(v => v.Id.Equals(id));
    }

    public async Task<IEnumerable<VehicleModel>> GetAllVehicleModelsOrderedByVehicleMarkNameAsync(
        bool noTracking = true)
    {
        return await CreateQuery(noTracking).OrderBy(v => v.VehicleMark!.VehicleMarkName)
            .ThenBy(v => v.VehicleModelName).ToListAsync();
    }

    public IEnumerable<VehicleModel> GetAllVehicleModelsOrderedByVehicleMarkName(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(v => v.VehicleMark!.VehicleMarkName)
            .ThenBy(v => v.VehicleModelName).ToList();
    }

    public async Task<List<VehicleModel>> GettingVehicleModelsByMarkIdAsync(Guid markId, bool noTracking = true)
    {
        return await CreateQuery(noTracking).Where(v => v.VehicleMarkId.Equals(markId))
            .OrderBy(v => v.VehicleMark!.VehicleMarkName).ToListAsync();
    }

    public List<VehicleModel> GettingVehicleModels(Guid markId, bool noTracking = true)
    {
        return CreateQuery(noTracking).Where(v => v.VehicleMarkId.Equals(markId))
            .OrderBy(v => v.VehicleMark!.VehicleMarkName).ToList();
    }

    protected override IQueryable<VehicleModel> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();

        query = query.Include(c => c.VehicleMark);
        return query;
    }
}*/