using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IAdminRepository: IEntityRepository<Admin>
{
    Task<IEnumerable<Admin>> GetAllAdminsOrderedByLastNameAsync(bool noTracking = true);
    IEnumerable<Admin> GetAllAdminsOrderedByLastName(bool noTracking = true);
}