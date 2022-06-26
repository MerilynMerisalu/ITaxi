using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class PhotoRepository: BaseEntityRepository<Photo, AppDbContext>, IPhotoRepository
{
    public PhotoRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}