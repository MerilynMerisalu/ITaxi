using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IVehicleRepository: IEntityRepository<Vehicle>
{
    List<int> GettingManufactureYears();
    
}