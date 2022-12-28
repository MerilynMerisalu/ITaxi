using System.Diagnostics;
using System.Linq.Expressions;
using Base.Contracts;
using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Base.Contracts.Domain;

namespace Base.BLL;

public class BaseEntityService<TBllEntity, TDalEntity, TRepository>: 
    BaseEntityService<TBllEntity, TDalEntity,TRepository, Guid>, IEntityService<TBllEntity>
    where TDalEntity : class, IDomainEntityId 
    where TBllEntity : class, IDomainEntityId 
    where TRepository : IEntityRepository<TDalEntity>
{
    public BaseEntityService(TRepository repository, IMapper<TBllEntity, TDalEntity> mapper) : base(repository, mapper)
    {
    }
}

public class BaseEntityService<TBllEntity, TDalEntity,TRepository, TKey>: 
    IEntityService<TBllEntity, TKey>
    where TDalEntity : class, IDomainEntityId, IDomainEntityId<TKey> 
    where TKey : IEquatable<TKey> 
    where TBllEntity : class, IDomainEntityId, IDomainEntityId<TKey>
    where TRepository: IEntityRepository<TDalEntity, TKey>
{
    protected TRepository Repository;
    protected IMapper<TBllEntity, TDalEntity> Mapper;

    public BaseEntityService(TRepository repository, IMapper<TBllEntity, TDalEntity> mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }
    public TBllEntity Add(TBllEntity entity)
    {
        return Mapper.Map(Repository.Add(Mapper.Map(entity)!))!;
    }

    public async Task<List<TBllEntity>> AddRangeAsync(List<TBllEntity> entities)
    {
        var dalEntities = new List<TDalEntity>();
        foreach (var entity in entities)
        {
            dalEntities.Add(Mapper.Map(entity)!);
        }

        var tBllEntities = new List<TBllEntity>();
        foreach (var tDalEntity in dalEntities)
        {
            tBllEntities.Add(Mapper.Map(tDalEntity)!);
        }

        return tBllEntities;
    }

    public TBllEntity Update(TBllEntity entity)
    {
        return Mapper.Map(Repository.Update(Mapper.Map(entity)!))!;
    }

    public TBllEntity Remove(TBllEntity entity)
    {
        return Mapper.Map(Repository.Remove(Mapper.Map(entity)!))!;
    }

    public TBllEntity Remove(TKey id)
    {
        return Mapper.Map(Repository.Remove(id))!;
    }

    public List<TBllEntity> RemoveAll(List<TBllEntity> entities)
    {
        var dalEntities = new List<TDalEntity>();
        foreach (var entity in entities)
        {
            dalEntities.Add(Mapper.Map(entity)!);
        }

        var tBllEntities = new List<TBllEntity>();
        
        Repository.RemoveAll(dalEntities);
        foreach (var dalEntity in dalEntities)
        {
            tBllEntities.Add(Mapper.Map(dalEntity)!);
        }
        

        return tBllEntities;
    }

    public TBllEntity? FirstOrDefault(TKey id, bool noTracking = true)
    {
        return Mapper.Map(Repository.FirstOrDefault(id, noTracking));
    }

    public IEnumerable<TBllEntity> GetAll(bool noTracking = true)
    {
        return Repository.GetAll(noTracking).Select(e => Mapper.Map(e))!;
    }

    public bool Exists(TKey id)
    {
        return Repository.Exists(id);
    }
    #warning Ask about this
    public bool Any(Expression<Func<TBllEntity?, bool>> filter, bool noTracking = true)
    {
        throw new NotImplementedException();
        //return Repository.CreateQuery(noTracking).Any(filter, noTracking);
    }
    
    //public Task<bool> AnyAsync(Expression<Func<TBllEntity?, bool>> filter, bool noTracking = true)
    //{
    //    return Repository.AnyAsync(filter, noTracking);
    //}
#warning Ask about this
    public TBllEntity? SingleOrDefault(Expression<Func<TBllEntity?, bool>> filter, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public TBllEntity? First(bool noTracking = true)
    {
        return Mapper.Map(Repository.First(noTracking));
    }

    public List<TBllEntity> AddRange(List<TBllEntity> entities)
    {
        var dalEntities = new List<TDalEntity>();
        foreach (var entity in entities)
        {
            dalEntities.Add(Mapper.Map(entity)!);
        }

        Repository.AddRange(dalEntities);
        return entities;
    }

    public async Task<TBllEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true)
    {
        var dalEntity = await Repository.FirstOrDefaultAsync(id, noTracking);
        return Mapper.Map(dalEntity);
    }

    public async Task<IEnumerable<TBllEntity>> GetAllAsync(bool noTracking = true)
    {
        return (await Repository.GetAllAsync(noTracking)).Select(e => Mapper.Map(e))!;
    }

    public async Task<bool> ExistsAsync(TKey id)
    {
        return (await Repository.ExistsAsync(id));
    }

    public Task<TBllEntity> RemoveAsync(TKey id)
    {
        var t = Repository.Remove(id);
        return Task.FromResult(Mapper.Map(t)!);
    }
    
#warning Ask about this
    public async Task<bool> AnyAsync(Expression<Func<TBllEntity?, bool>> filter, bool noTracking = true)
    {
        throw new NotImplementedException();
    }
#warning Ask about this
    public async Task<TBllEntity?> SingleOrDefaultAsync(Expression<Func<TBllEntity?, bool>> filter, bool noTracking = true)
    {
        throw new NotImplementedException();
    }

    public async Task<TBllEntity?> FirstAsync(bool noTracking = true)
    {
        return Mapper.Map(await Repository.FirstAsync(noTracking));
    }
}