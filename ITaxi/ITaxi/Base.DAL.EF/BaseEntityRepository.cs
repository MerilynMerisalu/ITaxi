using System.Linq.Expressions;
using Base.Contracts;
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
    public BaseEntityRepository(TDbContext dbContext, IMapper<TAppEntity, TDalEntity> mapper) 
        : base(dbContext, mapper )
    {
    }
}

public class BaseEntityRepository<TAppEntity, TDalEntity, TKey, TDbContext> : 
    IEntityRepository<TAppEntity, TKey>
    where TAppEntity : class, IDomainEntityId<TKey>
    where TDalEntity: class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : DbContext
{
    protected readonly TDbContext RepoDbContext;
    protected readonly DbSet<TDalEntity> RepoDbSet;
    protected readonly IMapper<TAppEntity, TDalEntity> Mapper;
    
    
    public BaseEntityRepository(TDbContext dbContext, IMapper<TAppEntity, TDalEntity> mapper)
    {
        RepoDbContext = dbContext;
        Mapper = mapper;
        RepoDbSet = dbContext.Set<TDalEntity>();
    }


    public virtual TAppEntity Add(TAppEntity entity)
    {
        var dalEntity = Mapper.Map(entity);
        return Mapper.Map(RepoDbSet.Add(dalEntity!).Entity)!;
    }

    public async Task<List<TAppEntity>> AddRangeAsync(List<TAppEntity> entities)
    {
        List<TDalEntity> dalEntities = new List<TDalEntity>();
        foreach (var appEntity in entities)
        {
            dalEntities.Add(Mapper.Map(appEntity)!);
        }
        await RepoDbSet.AddRangeAsync(dalEntities);
        return entities;
    }

    public virtual TAppEntity Update(TAppEntity entity)
    {
        return Mapper.Map(RepoDbSet.Update(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TAppEntity Remove(TAppEntity entity)
    {
        return Mapper.Map(RepoDbSet.Remove(Mapper.Map(entity)!).Entity)!;
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
        List<TDalEntity> dalEntities = new List<TDalEntity>();
        foreach (var entity in entities)
        {
            dalEntities.Add(Mapper.Map(entity)!);
        }
        RepoDbSet.RemoveRange(dalEntities);
        return entities.ToList();
    }

    public virtual TAppEntity? FirstOrDefault(TKey id, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(noTracking)
            .FirstOrDefault(e =>e.Id.Equals(id)));
    }

    public virtual IEnumerable<TAppEntity> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).Select(e => Mapper.Map(e)).ToList()!;
    }

    public virtual bool Exists(TKey id)
    {
        return RepoDbSet.Any(a => a.Id.Equals(id));
    }

    public bool Any(Expression<Func<TAppEntity?, bool>> filter, bool noTracking = true)
    {
        return CreateQuery(noTracking)
            .Select(e => Mapper.Map(e)).Any(filter);
        
    }

    public TAppEntity? SingleOrDefault(Expression<Func<TAppEntity?, bool>> filter, bool noTracking = true)
    {
        return CreateQuery(noTracking).Select(e => Mapper.Map(e)).SingleOrDefault(filter);
        
        
    }

    /*public virtual bool Any(Func<TDalEntity, bool> filter, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var res = query.AsQueryable().Any(filter);
        return res;
    }*/

    public virtual TAppEntity? SingleOrDefault(Func<TDalEntity, bool> filter, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var result = query.Select(d => d).SingleOrDefault(filter);
        return Mapper.Map(result);
    }

    public TAppEntity? First(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        var result = query.First();
        return Mapper.Map(result);
    }

    public List<TAppEntity> AddRange(List<TAppEntity> entities)
    {
        var dalEntities = new List<TDalEntity>();
        foreach (var entity in entities)
        {
            dalEntities.Add(Mapper.Map(entity)!);
        }
        RepoDbSet.AddRange(dalEntities);
        return entities;
    }


    public virtual async Task<TAppEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true)
    {
        var dalEntity = await CreateQuery(noTracking).FirstOrDefaultAsync(e => e.Id.Equals(id));
        return Mapper.Map(dalEntity);
    }

    public virtual async Task<IEnumerable<TAppEntity>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).Select(e => Mapper.Map(e)).ToListAsync())!;
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
        return  await CreateQuery(noTracking).Select(e => Mapper.Map(e)).AnyAsync(filter);
        
        
    }
    

    public virtual async Task<TAppEntity?> SingleOrDefaultAsync(Expression<Func<TAppEntity?, bool>> filter, 
        bool noTracking = true)
    {
        return await CreateQuery(noTracking)
            .Select(e => Mapper.Map(e)).SingleOrDefaultAsync(filter);
    }

    public virtual async Task<TAppEntity?> FirstAsync(bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking).FirstAsync());
    }

    protected virtual IQueryable<TDalEntity> CreateQuery(bool noTracking = true)
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