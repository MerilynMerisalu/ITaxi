using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class ScheduleRepository: BaseEntityRepository<Schedule, AppDbContext>, IScheduleRepository
{
    public ScheduleRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    protected override IQueryable<Schedule> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query.AsNoTracking();
        }

        query = query.Include(c => c.Driver)
            .ThenInclude(a => a!.AppUser)
            .Include(s => s.Vehicle)
            .ThenInclude(s => s!.VehicleMark)
            .Include(s => s.Vehicle)
            .ThenInclude(s => s!.VehicleModel)
            .Include(v => v.Vehicle)
            .ThenInclude(v => v!.VehicleType);
        return query;
    }

    public override async Task<IEnumerable<Schedule>> GetAllAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).ToListAsync();
    }

    public override IEnumerable<Schedule> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList();
    }
    
    
}