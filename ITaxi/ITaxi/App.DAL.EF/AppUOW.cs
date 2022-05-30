using App.Contracts.DAL;
using App.Contracts.DAL.IAppRepositories;
using App.DAL.EF.Repositories;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW: BaseUOW<AppDbContext>, IAppUnitOfWork
{
    public AppUOW(AppDbContext dbContext) : base(dbContext)
    {
    }

    private ICountyRepository? _counties;
    private ICityRepository? _cities;

    public ICountyRepository Counties => _counties ?? new CountyRepository(UOWDbContext);
    public ICityRepository Cities => _cities ?? new CityRepository(UOWDbContext);
}