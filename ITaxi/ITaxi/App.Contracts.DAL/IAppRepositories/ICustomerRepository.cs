using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICustomerRepository : IEntityRepository<App.DAL.DTO.AdminArea.CustomerDTO>, 
    ICustomerRepositoryCustom<App.DAL.DTO.AdminArea.CustomerDTO>
{
    
}

public interface ICustomerRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GettingAllCustomersWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<TEntity?> GettingAllCustomersWithoutIncludes(bool noTracking = true);
    Task<IEnumerable<TEntity?>> GettingAllOrderedCustomersWithoutIncludesAsync(bool noTracking = true);
    Task<TEntity?> GettingCustomerByIdWithoutIncludesAsync(Guid id, bool noTracking = true);
    Task<IEnumerable<TEntity?>> GettingAllOrderedCustomersAsync(bool noTracking = true);
    IEnumerable<TEntity?> GettingAllOrderedCustomers(bool noTracking = true);
    TEntity? GettingCustomerByIdWithoutIncludes(Guid id, bool noTracking = true);
    Task<bool> HasBookingsAnyAsync(Guid customerId);
    bool HasAnyBookings(Guid customerId);
}