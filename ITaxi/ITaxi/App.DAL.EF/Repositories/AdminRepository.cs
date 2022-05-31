using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class AdminRepository: BaseEntityRepository<Admin, AppDbContext>, IAdminRepository
{
    public AdminRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}