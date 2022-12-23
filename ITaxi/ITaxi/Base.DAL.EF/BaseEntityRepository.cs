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
        : base(dbContext, mapper )
    {
    }
}

public class BaseEntityRepository<TDalEntity, TDomainEntity, TKey, TDbContext> : 
    IEntityRepository<TDalEntity, TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TDomainEntity: class, IDomainEntityId<TKey>
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
        /*
        TDomainEntity domainEntity = Mapper.Map(entity)!;
        //TDomainEntity removeResponse = RepoDbSet.Remove(domainEntity).Entity;
        var entry = RepoDbSet.Entry(domainEntity);
        entry.State = EntityState.Deleted;
        RepoDbContext.SaveChanges();
        
        return Mapper.Map(entry.Entity)!;
        */
        
        return Mapper.Map(RepoDbSet.Remove(Mapper.Map(entity)!).Entity)!;
        //return Mapper.Map(RepoDbContext.Remove(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Remove(TKey id)
    {
        var entity = FirstOrDefault(id);
        if (entity == null)
            // TODO: implement custom exception for entity not found
            throw new NullReferenceException($"Entity {typeof(TDalEntity).Name} with id {id} was not found");
        return Remove(entity);
    }

    public virtual List<TDalEntity> RemoveAll(List<TDalEntity> entities)
    {
        List<TDomainEntity> dalEntities = new List<TDomainEntity>();
        foreach (var entity in entities)
        {
            dalEntities.Add(Mapper.Map(entity)!);
        }
        RepoDbSet.RemoveRange(dalEntities);
        return entities.ToList();
    }

    public virtual TDalEntity? FirstOrDefault(TKey id, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(noTracking)
            .FirstOrDefault(e =>e.Id.Equals(id)));
    }

    public virtual IEnumerable<TDalEntity> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(e => Mapper.Map(e)!);
    }

    public virtual bool Exists(TKey id)
    {
        return RepoDbSet.Any(a => a.Id.Equals(id));
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

    public TDalEntity? First(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
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


    public virtual async Task<TDalEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true)
    {
        var dalEntity = await CreateQuery(noTracking).FirstOrDefaultAsync(e => e.Id.Equals(id));
        return Mapper.Map(dalEntity);
    }

    public virtual async Task<IEnumerable<TDalEntity>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).Select(e => Mapper.Map(e)).ToListAsync())!;
    }

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await RepoDbSet.AnyAsync(a => a.Id.Equals(id));
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

    public virtual async Task<TDalEntity?> FirstAsync(bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking).FirstAsync());
    }

    protected virtual IQueryable<TDomainEntity> CreateQuery(bool noTracking = true)
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