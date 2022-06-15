using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICommentRepository : IEntityRepository<Comment>
{
    
}