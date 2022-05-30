using System.Linq.Expressions;
using Base.Contracts.Domain;

namespace Base.Contracts.DAL;

public interface IEntityRepository<TEntity>:IEntityRepository<TEntity, Guid>
where TEntity: class, IDomainEntityId
{
    
    
}

public interface IEntityRepository<TEntity, TKey>
    where TEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
    TEntity Add(TEntity entity);
    TEntity Update(TEntity entity);
    TEntity Remove(TEntity entity);
    TEntity Remove(TKey id);


    List<TEntity> RemoveAll();
    TEntity? FirstOrDefault(TKey id, bool noTracking = true);
    IEnumerable<TEntity> GetAll(bool noTracking = true);
    bool Exists(TKey id);
    bool Any(Expression<Func<TEntity?, bool>> filter,  bool noTracking = true );

    // async
    Task<TEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllAsync(bool noTracking = true);
    Task<bool> ExistsAsync(TKey id);
    Task<TEntity> RemoveAsync(TKey id);
    Task<bool> AnyAsync(Expression<Func<TEntity?, bool>> filter, bool noTracking = true);

}