using App.Contracts.DAL.IAppRepositories;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork: IUnitOfWork
{
     ICountyRepository Counties { get; }
     ICityRepository Cities { get; }
    
}