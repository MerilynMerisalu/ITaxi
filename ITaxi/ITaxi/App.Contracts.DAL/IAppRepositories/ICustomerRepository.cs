using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.IAppRepositories;

public interface ICustomerRepository : IEntityRepository<App.DAL.DTO.AdminArea.CustomerDTO>
{
    Task<IEnumerable<CustomerDTO?>> GettingAllCustomersWithoutIncludesAsync(bool noTracking = true);
    IEnumerable<CustomerDTO?> GettingAllCustomersWithoutIncludes(bool noTracking = true);
    Task<IEnumerable<CustomerDTO?>> GettingAllOrderedCustomersWithoutIncludesAsync(bool noTracking = true);
    Task<CustomerDTO?> GettingCustomerByIdWithoutIncludesAsync(Guid id, bool noTracking = true);
    Task<IEnumerable<CustomerDTO?>> GettingAllOrderedCustomersAsync(bool noTracking = true);
    IEnumerable<CustomerDTO?> GettingAllOrderedCustomers(bool noTracking = true);
    CustomerDTO? GettingCustomerByIdWithoutIncludes(Guid id, bool noTracking = true);
}