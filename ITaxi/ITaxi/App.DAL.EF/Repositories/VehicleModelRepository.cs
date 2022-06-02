using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.Contracts.DAL;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class VehicleModelRepository: BaseEntityRepository<VehicleModel, AppDbContext>, IVehicleModelRepository
{
    public VehicleModelRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}