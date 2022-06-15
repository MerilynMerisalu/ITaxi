using System.Linq.Expressions;
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
    
    

    public virtual TEntity Add(TEntity entity)
    {
        return RepoDbSet.Add(entity).Entity;
    }

    public async Task<List<TEntity>> AddRangeAsync(List<TEntity> entities)
    {
        await RepoDbSet.AddRangeAsync(entities);
        return entities;
    }

    public virtual TEntity Update(TEntity entity)
    {
        return RepoDbSet.Update(entity).Entity;
    }

    public virtual TEntity Remove(TEntity entity)
    {
        return RepoDbSet.Remove(entity).Entity;
    }

    public virtual TEntity Remove(TKey id)
    {
        var entity = FirstOrDefault(id);
        if (entity == null)
        {
            // TODO: implement custom exception for entity not found
            throw new NullReferenceException($"Entity {typeof(TEntity).Name} with id {id} was not found");
        }
        return Remove(entity);
    }

    public virtual List<TEntity> RemoveAll(List<TEntity> entities)
    {
        
        RepoDbSet.RemoveRange(entities);
        return entities.ToList();
    }

    public virtual TEntity? FirstOrDefault(TKey id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(e => e.Id.Equals(id));
    }

    public virtual IEnumerable<TEntity> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList();
    }

    public virtual bool Exists(TKey id)
    {
        return RepoDbSet.Any(a => a.Id.Equals(id));
    }

    public virtual bool Any(Expression<Func<TEntity?, bool>> filter, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var res = query.AsQueryable().Any(filter);
        return res;
    }

    public virtual TEntity? SingleOrDefault(Expression<Func<TEntity?, bool>> filter, bool noTracking = true)
    {
        var query =  CreateQuery(noTracking);
        var result = query.Select(d => d).SingleOrDefault(filter);
        return result;

    }

    public TEntity? First(bool noTracking = true)
    {
        var query =  CreateQuery( noTracking);
        var result = query.First();
        return result;

    }

    public List<TEntity> AddRange(List<TEntity> entities)
    {
        RepoDbSet.AddRange(entities);
        return entities;
    }


    public virtual async Task<TEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true)
    {
        return await CreateQuery(noTracking).FirstOrDefaultAsync(e => e.Id.Equals(id));
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).ToListAsync();
    }

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await RepoDbSet.AnyAsync(a => a.Id.Equals(id));
    }

    public virtual async Task<TEntity> RemoveAsync(TKey id)
    {
        var entity = await FirstOrDefaultAsync(id);
        if (entity == null)
        {
            // TODO: implement custom exception for entity not found
            throw new NullReferenceException($"Entity {typeof(TEntity).Name} with id {id} was not found");
        }
        return Remove(entity);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TEntity?, bool>> filter, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var res = await query.AsQueryable().AnyAsync(filter);
        return res;
    }

    public virtual async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity?, bool>> filter, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var result = await query.Select(d => d).SingleOrDefaultAsync(filter);
        return result;
    }

    public virtual async Task<TEntity?> FirstAsync(bool noTracking = true)
    {
        var query =  CreateQuery( noTracking);
        var result = await query.FirstAsync();
        return result;

    }
}