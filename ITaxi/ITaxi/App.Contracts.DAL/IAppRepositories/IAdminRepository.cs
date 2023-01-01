
using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IAdminRepository : IEntityRepository<AdminDTO>
{
    Task<IEnumerable<AdminDTO>> GetAllAdminsOrderedByLastNameAsync(bool noTracking = true);
    IEnumerable<AdminDTO> GetAllAdminsOrderedByLastName(bool noTracking = true);
}