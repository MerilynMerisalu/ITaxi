using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class VehicleTypeRepository: BaseEntityRepository<VehicleType, AppDbContext>, IVehicleTypeRepository
{
    public VehicleTypeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}