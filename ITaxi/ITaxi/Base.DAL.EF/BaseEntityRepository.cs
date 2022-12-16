using System.Linq.Expressions;
using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class BaseEntityRepository<TAppEntity, TDalEntity, TDbContext> : 
    BaseEntityRepository<TAppEntity, TDalEntity, Guid, TDbContext>
    where TAppEntity : class, IDomainEntityId<Guid>
    where TDalEntity : class, IDomainEntityId<Guid>
    where TDbContext : DbContext
{
    public BaseEntityRepository(TDbContext dbContext) : base(dbContext )
    {
    }
}

public class BaseEntityRepository<TAppEntity, TDalEntity, TKey, TDbContext> : 
    IEntityRepository<TAppEntity, TDalEntity, TKey>
    where TAppEntity : class, IDomainEntityId<TKey>
    where TDalEntity: class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : DbContext
{
    protected readonly TDbContext RepoDbContext;
    protected readonly DbSet<TDalEntity> RepoDbSet;

    public BaseEntityRepository(TDbContext dbContext)
    {
        RepoDbContext = dbContext;
        RepoDbSet = dbContext.Set<TDalEntity>();
    }


    public virtual TAppEntity Add(TAppEntity entity)
    {
        return RepoDbSet.Add(entity).Entity;
    }

    public async Task<List<TAppEntity>> AddRangeAsync(List<TAppEntity> entities)
    {
        await RepoDbSet.AddRangeAsync(entities);
        return entities;
    }

    public virtual TAppEntity Update(TAppEntity entity)
    {
        return RepoDbSet.Update(entity).Entity;
    }

    public virtual TAppEntity Remove(TAppEntity entity)
    {
        return RepoDbSet.Remove(entity).Entity;
    }

    public virtual TAppEntity Remove(TKey id)
    {
        var entity = FirstOrDefault(id);
        if (entity == null)
            // TODO: implement custom exception for entity not found
            throw new NullReferenceException($"Entity {typeof(TAppEntity).Name} with id {id} was not found");
        return Remove(entity);
    }

    public virtual List<TAppEntity> RemoveAll(List<TAppEntity> entities)
    {
        RepoDbSet.RemoveRange(entities);
        return entities.ToList();
    }

    public virtual TAppEntity? FirstOrDefault(TKey id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(e => e.Id.Equals(id));
    }

    public virtual IEnumerable<TAppEntity> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList();
    }

    public virtual bool Exists(TKey id)
    {
        return RepoDbSet.Any(a => a.Id.Equals(id));
    }

    public virtual bool Any(Expression<Func<TAppEntity?, bool>> filter, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var res = query.AsQueryable().Any(filter);
        return res;
    }

    public virtual TAppEntity? SingleOrDefault(Expression<Func<TAppEntity?, bool>> filter, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var result = query.Select(d => d).SingleOrDefault(filter);
        return result;
    }

    public TAppEntity? First(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var result = query.First();
        return result;
    }

    public List<TAppEntity> AddRange(List<TAppEntity> entities)
    {
        RepoDbSet.AddRange(entities);
        return entities;
    }


    public virtual async Task<TAppEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true)
    {
        return await CreateQuery(noTracking).FirstOrDefaultAsync(e => e.Id.Equals(id));
    }

    public virtual async Task<IEnumerable<TAppEntity>> GetAllAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).ToListAsync();
    }

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await RepoDbSet.AnyAsync(a => a.Id.Equals(id));
    }

    public virtual async Task<TAppEntity> RemoveAsync(TKey id)
    {
        var entity = await FirstOrDefaultAsync(id);
        if (entity == null)
            // TODO: implement custom exception for entity not found
            throw new NullReferenceException($"Entity {typeof(TAppEntity).Name} with id {id} was not found");
        return Remove(entity);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TAppEntity?, bool>> filter, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var res = await query.AsQueryable().AnyAsync(filter);
        return res;
    }
    

    public virtual async Task<TAppEntity?> SingleOrDefaultAsync(Expression<Func<TAppEntity?, bool>> filter, 
        bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var result = await query.Select(d => d).SingleOrDefaultAsync(filter);
        return result;
    }

    public virtual async Task<TAppEntity?> FirstAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var result = await query.FirstAsync();
        return result;
    }

    protected virtual IQueryable<TAppEntity> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking)
        {
            query = query.AsQueryable().AsNoTracking();
            return query;
        }

        return query;
    }
}