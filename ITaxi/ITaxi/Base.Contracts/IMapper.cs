﻿using System.Linq.Expressions;

namespace Base.Contracts;

public interface IMapper<TOut, TIn>
{
    TOut? Map(TIn? entity);
    TIn? Map(TOut? entity);
    //IQueryable<TIn> MapQuery(IQueryable<TOut> entityQuery);
    //IQueryable<TOut> MapQuery(IQueryable<TIn> entityQuery);

}