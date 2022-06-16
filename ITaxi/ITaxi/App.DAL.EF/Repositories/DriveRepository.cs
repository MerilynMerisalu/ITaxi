using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class DriveRepository: BaseEntityRepository<Drive, AppDbContext>, IDriveRepository
{
    public DriveRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    protected override IQueryable<Drive> CreateQuery(bool noTracking = true)
    {
        return base.CreateQuery(noTracking);
    }

    public override Task<IEnumerable<Drive>> GetAllAsync(bool noTracking = true)
    {
        return base.GetAllAsync(noTracking);
    }

    public override IEnumerable<Drive> GetAll(bool noTracking = true)
    {
        return base.GetAll(noTracking);
    }

    public override Task<Drive?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return base.FirstOrDefaultAsync(id, noTracking);
    }

    public override Drive? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return base.FirstOrDefault(id, noTracking);
    }

    public async Task<IEnumerable<Drive>> GetAllDrivesWithoutIncludesAsync(bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Drive> GetAllDrivesWithoutIncludes(bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Drive>> GettingAllOrderedSchedulesWithIncludesAsync(bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Drive> GettingAllOrderedDrivesWithIncludes(bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Drive>> GettingAllOrderedSchedulesWithoutIncludesAsync(bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Drive> GettingAllOrderedDrivesWithoutIncludes(bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public async Task<Drive?> GettingDriveWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public Drive? GetDriveWithoutIncludes(Guid id, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Drive?>> SearchByDateAsync(DateTime search)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Drive?> SearchByDate(DateTime search)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Drive?>> PrintAsync()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Drive?> Print()
    {
        throw new NotImplementedException();
    }
}