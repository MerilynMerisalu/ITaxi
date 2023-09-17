﻿using App.DAL.DTO.AdminArea;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICountryRepository: IEntityRepository<CountryDTO>, ICountryRepositoryCustom<CountryDTO>
{
    
}

public interface ICountryRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllCountriesOrderedByCountryNameAsync(bool noTracking = true, bool noIncludes = false);
    IEnumerable<TEntity> GetAllCountriesOrderedByCountryName(bool noTracking = true, bool noIncludes = false);
    Task<bool> HasAnyCountiesAsync(Guid id, bool noTracking = true);
    bool HasAnyCounties(Guid id, bool noTracking = true);


}