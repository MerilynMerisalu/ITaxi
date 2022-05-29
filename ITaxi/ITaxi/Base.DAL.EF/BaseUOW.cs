using Base.Contracts.DAL;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class BaseUOW<TDbContext>: IUnitOfWork 
    where TDbContext: DbContext

{
    protected readonly TDbContext UOWDbContext;

    public BaseUOW( TDbContext uowDbContext)
    {
        UOWDbContext = uowDbContext;
    }
    public async Task<int> SaveChangesAsync()
    {
        return await UOWDbContext.SaveChangesAsync();
    }

    public int SaveChanges()
    {
        return UOWDbContext.SaveChanges();
    }
}