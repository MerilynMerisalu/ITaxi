using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class VehicleRepository: BaseEntityRepository<Vehicle, AppDbContext>, IVehicleRepository
{
    public VehicleRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    protected override IQueryable<Vehicle> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query.AsNoTracking();
        }

        query = query.Include(c => c.Driver)
            .ThenInclude(d => d!.AppUser)
            .Include(v => v.VehicleMark)
            .Include(v => v.VehicleModel)
            .Include(v => v.VehicleType);
        return query;
    }

    public override async Task<IEnumerable<Vehicle>> GetAllAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).ToListAsync();
    }

    public override IEnumerable<Vehicle> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList();
    }

    public override Task<Vehicle?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefaultAsync(v => v.Id.Equals(id));
    }

    public override Vehicle? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(v => v.Id.Equals(id));
    }

    public List<int> GettingManufactureYears()
    {
        var years = new List<int>();

        for (var i = 6; i > 0; i--)
        {
            var year = DateTime.Today.AddYears(-i).Year;
            years.Add(year);
        }

        years.Reverse();

        return years;
    }

    
}