using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class VehicleRepository: BaseEntityRepository<Vehicle, AppDbContext>, IVehicleRepository
{
    public VehicleRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}