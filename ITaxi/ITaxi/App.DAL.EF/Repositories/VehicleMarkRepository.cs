using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class VehicleMarkRepository: BaseEntityRepository<VehicleMark, AppDbContext>, IVehicleMarkRepository
{
    public VehicleMarkRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}