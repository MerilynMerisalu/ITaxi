using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICustomerRepository : IEntityRepository<Customer>
{
    Task<IEnumerable<Customer?>> GettingAllCustomersWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<Customer?> GettingAllCustomersWithoutIncludes(bool noTracking = true);
    Task<IEnumerable<Customer?>> GettingAllOrderedCustomersWithoutIncludesAsync(bool noTracking = true);
    Task<Customer?> GettingCustomerByIdWithoutIncludesAsync(Guid id, bool noTracking = true);
    Task<IEnumerable<Customer?>> GettingAllOrderedCustomersAsync(bool noTracking = true);
    IEnumerable<Customer?> GettingAllOrderedCustomers(bool noTracking = true);
    Customer? GettingCustomerByIdWithoutIncludes(Guid id, bool noTracking = true);
}