using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface IPhotoRepository: IEntityRepository<Photo>
{
    // Add custom methods here
}