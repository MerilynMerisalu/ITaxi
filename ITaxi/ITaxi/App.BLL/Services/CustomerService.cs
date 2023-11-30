using App.BLL.DTO.AdminArea;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class CustomerService: BaseEntityService<App.BLL.DTO.AdminArea.CustomerDTO, App.DAL.DTO.AdminArea.CustomerDTO,
ICustomerRepository>, ICustomerService
{
    public CustomerService(ICustomerRepository repository, IMapper<CustomerDTO, DAL.DTO.AdminArea.CustomerDTO> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<CustomerDTO?>> GettingAllCustomersWithoutIncludesAsync(bool noTracking = true)
    {
        return (await Repository.GettingAllCustomersWithoutIncludesAsync(noTracking)).Select(e => Mapper.Map(e));
    }

    public IEnumerable<CustomerDTO?> GettingAllCustomersWithoutIncludes(bool noTracking = true)
    {
        return Repository.GettingAllCustomersWithoutIncludes(noTracking).Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<CustomerDTO?>> GettingAllOrderedCustomersWithoutIncludesAsync(bool noTracking = true)
    {
        return (await Repository.GettingAllOrderedCustomersWithoutIncludesAsync(noTracking)).Select(e => Mapper.Map(e));
    }

    public async Task<CustomerDTO?> GettingCustomerByIdWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.GettingCustomerByIdWithoutIncludesAsync(id, noTracking));
    }

    public async Task<IEnumerable<CustomerDTO?>> GettingAllOrderedCustomersAsync(bool noTracking = true)
    {
        return (await Repository.GettingAllOrderedCustomersAsync(noTracking)).Select(e => Mapper.Map(e));
    }

    public IEnumerable<CustomerDTO?> GettingAllOrderedCustomers(bool noTracking = true)
    {
        return Repository.GettingAllOrderedCustomers(noTracking).Select(e => Mapper.Map(e));
    }

    public CustomerDTO? GettingCustomerByIdWithoutIncludes(Guid id, bool noTracking = true)
    {
        return Mapper.Map(Repository.GettingCustomerByIdWithoutIncludes(id, noTracking));
    }

    public async Task<bool> HasBookingsAnyAsync(Guid customerId)
    {
        return await Repository.HasBookingsAnyAsync(customerId);
    }

    public bool HasAnyBookings(Guid customerId)
    {
        return Repository.HasAnyBookings(customerId);
    }

    public async Task<Guid> GettingCustomerIdByAppUserIdAsync(Guid appUserId)
    {
        return await Repository.GettingCustomerIdByAppUserIdAsync(appUserId);
    }

    public Guid GettingCustomerIdByAppUserId(Guid appUserId)
    {
        return Repository.GettingCustomerIdByAppUserId(appUserId);
    }
}