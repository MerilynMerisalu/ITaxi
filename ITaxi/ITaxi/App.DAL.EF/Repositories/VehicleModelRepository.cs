﻿using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class VehicleModelRepository: BaseEntityRepository<VehicleModel, AppDbContext>, IVehicleModelRepository
{
    public VehicleModelRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    protected override IQueryable<VehicleModel> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query.AsNoTracking();
        }

        query = query.Include(c => c.VehicleMark);
        return query;
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

    public async Task<IEnumerable<VehicleModel>> GetAllVehicleModelsOrderedByVehicleMarkNameAsync(bool noTracking = true)
    {
        return await base.CreateQuery(noTracking).ToListAsync();
    }

    public IEnumerable<VehicleModel> GetAllVehicleModelsOrderedByVehicleMarkName(bool noTracking = true)
    {
        return base.CreateQuery(noTracking).ToList();
    }
}