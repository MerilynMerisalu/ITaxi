using AutoMapper;
using Base.Contracts;

namespace Base.DAL;

public class BaseMapper<TOut, TIn>: IMapper<TOut,TIn>
{
    protected readonly AutoMapper.IMapper Mapper;

    public BaseMapper(IMapper mapper)
    {
        Mapper = mapper;
    }

    public TOut? Map(TIn? entity)
    {
        return Mapper.Map<TOut>(entity);
    }

    public TIn? Map(TOut? entity)
    {
        return Mapper.Map<TIn>(entity);
    }
    
    //public IQueryable<TOut> MapQuery(IQueryable<TIn> entityQuery)
    //{
    //    return Mapper.ProjectTo<TOut>(entityQuery);
    //}
    //
    //public IQueryable<TIn> MapQuery(IQueryable<TOut> entityQuery)
    //{
    //    return Mapper.ProjectTo<TIn>(entityQuery);
    //}
}