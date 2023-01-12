using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class VehicleModelRepository : BaseEntityRepository<App.DAL.DTO.AdminArea.VehicleModelDTO, App.Domain.VehicleModel, AppDbContext>, IVehicleModelRepository
{
    public VehicleModelRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.AdminArea.VehicleModelDTO, App.Domain.VehicleModel> mapper) : base(dbContext, mapper)
    {
    }


    public async Task<IEnumerable<VehicleModelDTO>> GetAllVehicleModelsWithoutVehicleMarksAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(e=> Mapper.Map(e))!;
    }

    public IEnumerable<VehicleModelDTO> GetAllVehicleModelsWithoutVehicleMarks(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(e=> Mapper.Map(e))!;
    }

    public async Task<VehicleModelDTO?> FirstOrDefaultVehicleModelWithoutVehicleMarkAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking).FirstOrDefaultAsync(v => v.Id.Equals(id)));
    }

    public VehicleModelDTO? FirstOrDefaultVehicleModelWithoutVehicleMark(Guid id, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(noTracking).FirstOrDefault(v => v.Id.Equals(id)));
    }

    public async Task<IEnumerable<VehicleModelDTO>> GetAllVehicleModelsOrderedByVehicleMarkNameAsync(
        bool noTracking = true)
    {
        return (await CreateQuery(noTracking).OrderBy(v => v.VehicleMark!.VehicleMarkName)
            .ThenBy(v => v.VehicleModelName).ToListAsync()).Select(e=> Mapper.Map(e))!;
    }

    public IEnumerable<VehicleModelDTO> GetAllVehicleModelsOrderedByVehicleMarkName(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(v => v.VehicleMark!.VehicleMarkName)
            .ThenBy(v => v.VehicleModelName).ToList().Select(e=> Mapper.Map(e))!;
    }

    public async Task<List<VehicleModelDTO>> GettingVehicleModelsByMarkIdAsync(Guid markId, bool noTracking = true)
    {
        return ((await CreateQuery(noTracking).Where(v => v.VehicleMarkId.Equals(markId))
            .OrderBy(v => v.VehicleMark!.VehicleMarkName).ToListAsync()).Select(e=> Mapper.Map(e)) as List<VehicleModelDTO>)!;
    }

    public List<VehicleModelDTO> GettingVehicleModels(Guid markId, bool noTracking = true)
    {
        return (CreateQuery(noTracking).Where(v => v.VehicleMarkId.Equals(markId))
            .OrderBy(v => v.VehicleMark!.VehicleMarkName).ToList().Select(e=> Mapper.Map(e)) as List<VehicleModelDTO>)!;
    }

    public async Task<bool> HasAnyVehicleMarksAsync(Guid markId, bool noTracking = true)
    {
        return await CreateQuery(noTracking).AnyAsync(mo => mo.VehicleMarkId.Equals(markId));
    }

    public bool HasAnyVehicleMarks(Guid markId, bool noTracking = true)
    {
        return CreateQuery(noTracking).Any(mo => mo.VehicleMarkId.Equals(markId));
    }

    public async Task<bool> HasAnyModelsAsync(Guid markId, bool noTracking = true)
    {
        return await CreateQuery(noTracking).AnyAsync(v => v.VehicleMarkId.Equals(markId));
    }

    protected override IQueryable<VehicleModel> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();

        query = query.Include(c => c.VehicleMark);
        return query;
    }
}