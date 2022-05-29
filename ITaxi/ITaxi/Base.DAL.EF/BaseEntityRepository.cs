using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class BaseEntityRepository<TEntity, TDbContext>: BaseEntityRepository<TEntity, Guid, TDbContext>
    where TEntity: class, IDomainEntityId<Guid>
    where TDbContext: DbContext
{
    public BaseEntityRepository(TDbContext dbContext) : base(dbContext)
    {
    }
}

public class BaseEntityRepository<TEntity, TKey, TDbContext> : IEntityRepository<TEntity, TKey>
    where TEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : DbContext
{
    protected readonly TDbContext RepoDbContext;
    protected readonly DbSet<TEntity> RepoDbSet;

    public BaseEntityRepository(TDbContext dbContext)
    {
        RepoDbContext = dbContext;
        RepoDbSet = dbContext.Set<TEntity>();
    }

    protected virtual IQueryable<TEntity> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query = query.AsQueryable().AsNoTracking();
            return query;
        }

        return query;
    }

    public TEntity Add(TEntity entity)
    {
        return RepoDbSet.Add(entity).Entity;
    }

    public TEntity Update(TEntity entity)
    {
        return RepoDbSet.Update(entity).Entity;
    }

    public TEntity Remove(TEntity entity)
    {
        return RepoDbSet.Remove(entity).Entity;
    }

    public TEntity Remove(TKey id)
    {
        var entity = FirstOrDefault(id);
        if (entity == null)
        {
            // TODO: implement custom exception for entity not found
            throw new NullReferenceException($"Entity {typeof(TEntity).Name} with id {id} was not found");
        }
        return Remove(entity);
    }

    public List<TEntity> RemoveAll()
    {
        var entities = CreateQuery();
        RepoDbSet.RemoveRange(entities);
        return entities.ToList();
    }

    public TEntity? FirstOrDefault(TKey id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(e => e.Id.Equals(id));
    }

    public IEnumerable<TEntity> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList();
    }

    public bool Exists(TKey id)
    {
        return RepoDbSet.Any(a => a.Id.Equals(id));
    }

    public async Task<TEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true)
    {
        return await CreateQuery(noTracking).FirstOrDefaultAsync(e => e.Id.Equals(id));
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).ToListAsync();
    }

    public async Task<bool> ExistsAsync(TKey id)
    {
        return await RepoDbSet.AnyAsync(a => a.Id.Equals(id));
    }

    public async Task<TEntity> RemoveAsync(TKey id)
    {
        var entity = await FirstOrDefaultAsync(id);
        if (entity == null)
        {
            // TODO: implement custom exception for entity not found
            throw new NullReferenceException($"Entity {typeof(TEntity).Name} with id {id} was not found");
        }
        return Remove(entity);
    }
}