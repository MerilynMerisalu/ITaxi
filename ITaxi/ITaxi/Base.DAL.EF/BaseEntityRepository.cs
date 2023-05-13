using System.Linq.Expressions;
using Base.Contracts;
using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;
#warning do not fetch unnecessary data on every request
public class BaseEntityRepository<TDalEntity, TDomainEntity, TDbContext> :
    BaseEntityRepository<TDalEntity, TDomainEntity, Guid, TDbContext>
    where TDalEntity : class, IDomainEntityId<Guid>
    where TDomainEntity : class, IDomainEntityId<Guid>
    where TDbContext : DbContext
{
    public BaseEntityRepository(TDbContext dbContext, IMapper<TDalEntity, TDomainEntity> mapper)
        : base(dbContext, mapper)
    {
    }
}

public class BaseEntityRepository<TDalEntity, TDomainEntity, TKey, TDbContext> :
    IEntityRepository<TDalEntity, TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TDomainEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : DbContext
{
    protected readonly TDbContext RepoDbContext;
    protected readonly DbSet<TDomainEntity> RepoDbSet;
    protected readonly IMapper<TDalEntity, TDomainEntity> Mapper;


    public BaseEntityRepository(TDbContext dbContext, IMapper<TDalEntity, TDomainEntity> mapper)
    {
        RepoDbContext = dbContext;
        Mapper = mapper;
        RepoDbSet = dbContext.Set<TDomainEntity>();
    }


    public virtual TDalEntity Add(TDalEntity entity)
    {
        var dalEntity = Mapper.Map(entity);
        return Mapper.Map(RepoDbSet.Add(dalEntity!).Entity)!;
    }

    public async Task<List<TDalEntity>> AddRangeAsync(List<TDalEntity> entities)
    {
        List<TDomainEntity> dalEntities = new List<TDomainEntity>();
        foreach (var appEntity in entities)
        {
            dalEntities.Add(Mapper.Map(appEntity)!);
        }
        await RepoDbSet.AddRangeAsync(dalEntities);
        return entities;
    }

    public virtual TDalEntity Update(TDalEntity entity)
    {
        return Mapper.Map(RepoDbSet.Update(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Remove(TDalEntity entity)
    {
        // This is HARD Delete:
        //return Mapper.Map(RepoDbSet.Remove(Mapper.Map(entity)!).Entity)!;
        
        // Instead, we want to implement soft delete, by setting the IsDeleted Flag
        var data = Mapper.Map(entity)!;
        data.IsDeleted = true;

        if (data is IDomainEntityMeta meta)
        {
            meta.DeletedAt = DateTime.Now.ToUniversalTime();
            meta.DeletedBy = "?";
        }
        
        // TODO: also set the DeletedBy and DeletedAt
        
        return Mapper.Map(RepoDbSet.Update(data).Entity)!;
    }

    public virtual TDalEntity Remove(TKey id)
    {
        var entity = FirstOrDefault(id, noTracking:true, noIncludes:true);
        if (entity == null)
            // TODO: implement custom exception for entity not found
            throw new NullReferenceException($"Entity {typeof(TDalEntity).Name} with id {id} was not found");
        return Remove(entity);
    }

    public virtual List<TDalEntity> RemoveAll(List<TDalEntity> entities)
    {
        List<TDomainEntity> domainEntities = new List<TDomainEntity>();
        foreach (var entity in entities)
        {
            domainEntities.Add(Mapper.Map(entity)!);
        }
        // This is HARD delete:
        //RepoDbSet.RemoveRange(domainEntities);
        
        //We want SOFT delete
        foreach (var entity in domainEntities)
        {
            entity.IsDeleted = true;
            if (entity is IDomainEntityMeta meta)
            {
                meta.DeletedAt = DateTime.Now.ToUniversalTime();
                meta.DeletedBy = "?";
            }
        }
        RepoDbSet.UpdateRange(domainEntities);
        
        return entities.ToList();
    }

    public virtual TDalEntity? FirstOrDefault(TKey id, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(CreateQuery(noTracking, noIncludes)
            .FirstOrDefault(e => e.Id.Equals(id)));
    }

    public virtual IEnumerable<TDalEntity> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(e => Mapper.Map(e)!);
    }

    public virtual bool Exists(TKey id)
    {
        return CreateQuery().Any(a => a.Id.Equals(id));
    }

    public bool Any(Expression<Func<TDalEntity?, bool>> filter, bool noTracking = true)
    {
        return CreateQuery(noTracking)
            .Select(e => Mapper.Map(e)).Any(filter);

    }

    public TDalEntity? SingleOrDefault(Expression<Func<TDalEntity?, bool>> filter, bool noTracking = true)
    {
        return CreateQuery(noTracking).Select(e => Mapper.Map(e)).SingleOrDefault(filter);

    }

    /*public virtual bool Any(Func<TDalEntity, bool> filter, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var res = query.AsQueryable().Any(filter);
        return res;
    }*/

    public virtual TDalEntity? SingleOrDefault(Func<TDomainEntity, bool> filter, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var result = query.Select(d => d).SingleOrDefault(filter);
        return Mapper.Map(result);
    }

    public TDalEntity? First(bool noTracking = true, bool noIncludes = false)
    {
        var query = CreateQuery(noTracking, noIncludes);
        var result = query.First();
        return Mapper.Map(result);
    }

    public List<TDalEntity> AddRange(List<TDalEntity> entities)
    {
        var dalEntities = new List<TDomainEntity>();
        foreach (var entity in entities)
        {
            dalEntities.Add(Mapper.Map(entity)!);
        }
        RepoDbSet.AddRange(dalEntities);
        return entities;
    }


    public virtual async Task<TDalEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true, bool noIncludes = false)
    {
        var dalEntity = await CreateQuery(noTracking, noIncludes).FirstOrDefaultAsync(e => e.Id.Equals(id));
        return Mapper.Map(dalEntity);
    }

    public virtual async Task<IEnumerable<TDalEntity>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).Select(e => Mapper.Map(e)).ToListAsync())!;
    }

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await CreateQuery().AnyAsync(a => a.Id.Equals(id));
    }

    public virtual async Task<TDalEntity> RemoveAsync(TKey id)
    {
        var entity = await FirstOrDefaultAsync(id);
        if (entity == null)
            // TODO: implement custom exception for entity not found
            throw new NullReferenceException($"Entity {typeof(TDalEntity).Name} with id {id} was not found");
        return Remove(entity);
    }

    public virtual async Task<bool> AnyAsync(Expression<Func<TDalEntity?, bool>> filter, bool noTracking = true)
    {
        return await CreateQuery(noTracking).Select(x => Mapper.Map(x)).Where(filter).AnyAsync();
    }

    public virtual async Task<TDalEntity?> SingleOrDefaultAsync(Expression<Func<TDalEntity?, bool>> filter,
        bool noTracking = true)
    {
        return await CreateQuery(noTracking)
            .Select(e => Mapper.Map(e)).SingleOrDefaultAsync(filter);
    }

    public virtual async Task<TDalEntity?> FirstAsync(bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await CreateQuery(noTracking, noIncludes).FirstAsync());
    }

    /// <summary>
    /// Create a query against this Repository that sets up the Tracking and default Includes based onthe passed in parameters.
    /// </summary>
    /// <remarks>This method manages filtering for records that have been "Soft Deleted", inheriting classes should call this base implementation <i><b>first.</b></i></remarks>
    /// <param name="noTracking">Disable tracking on the query results if this is set to true</param>
    /// <param name="noIncludes">Ignore Auto Includes if this is true</param>
    /// <param name="showDeleted">If True, include "Soft Deleted" records in the query results</param>
    /// <returns>The default query that you can further compose for your domain repo logic</returns>
    protected virtual IQueryable<TDomainEntity> CreateQuery(bool noTracking = true, bool noIncludes = false,
        bool showDeleted = false)

    {
        var query = RepoDbSet.AsQueryable();
        if (!showDeleted)
            query = query.Where(x => !x.IsDeleted);
        if (noTracking)
            query = query.AsNoTracking();
        if (noIncludes)
            query = query.IgnoreAutoIncludes();

        return query;
    }
}