using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CustomerRepository : BaseEntityRepository<App.DAL.DTO.AdminArea.CustomerDTO, App.Domain.Customer, AppDbContext>, ICustomerRepository
{
    public CustomerRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.AdminArea.CustomerDTO, App.Domain.Customer> mapper) : base(dbContext, mapper)
    {
    }

    public override async Task<IEnumerable<CustomerDTO>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public override IEnumerable<CustomerDTO> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(e => Mapper.Map(e))!;
    }

    public  async Task<CustomerDTO?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking).FirstOrDefaultAsync(c => c.Id.Equals(id)));
    }

    public CustomerDTO? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(noTracking).FirstOrDefault(c => c.Id.Equals(id)));
    }

    public async Task<IEnumerable<CustomerDTO?>> GettingAllCustomersWithoutIncludesAsync(bool noTracking = true)
    {
        return (await base.CreateQuery(noTracking, noIncludes: true)
            .ToListAsync()).Select(e => Mapper.Map(e));
    }

    public IEnumerable<CustomerDTO?> GettingAllCustomersWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking, noIncludes: true).ToList().Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<CustomerDTO?>> GettingAllOrderedCustomersWithoutIncludesAsync(bool noTracking = true)
    {
        return (await base.CreateQuery(noTracking, noIncludes: true)
            .OrderBy(c => c.DisabilityType).ToListAsync()).Select(e => Mapper.Map(e));
    }

    public async Task<CustomerDTO?> GettingCustomerByIdWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await base.CreateQuery(noTracking, noIncludes: true)
            .FirstOrDefaultAsync(c => c.Id.Equals(id)));
    }

    public async Task<IEnumerable<CustomerDTO?>> GettingAllOrderedCustomersAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).OrderBy(c => c.AppUser!.LastName)
            .ThenBy(c => c.AppUser!.FirstName).ToListAsync()).Select(e => Mapper.Map(e));
    }

    public IEnumerable<CustomerDTO?> GettingAllOrderedCustomers(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(c => c.AppUser!.LastName)
            .ThenBy(c => c.AppUser!.FirstName).ToList().Select(e => Mapper.Map(e));
    }

    public CustomerDTO GettingCustomerByIdWithoutIncludes(Guid id, bool noTracking = true)
    {
        return Mapper.Map(base.CreateQuery(noTracking, noIncludes: true).FirstOrDefault(c => c.Id.Equals(id)))!;
    }

    public async Task<bool> HasBookingsAnyAsync(Guid customerId)
    {
        return await RepoDbContext.Bookings.AnyAsync(b => b.CustomerId.Equals(customerId));
    }

    public bool HasAnyBookings(Guid customerId)
    {
        return RepoDbContext.Bookings.Any(b => b.CustomerId.Equals(customerId));
    }

    public async Task<Guid> GettingCustomerIdByAppUserIdAsync(Guid appUserId)
    {
        return (await CreateQuery().SingleOrDefaultAsync(c => c.AppUserId.Equals(appUserId)))!.Id;
    }

    public Guid GettingCustomerIdByAppUserId(Guid appUserId)
    {
        return CreateQuery().SingleOrDefault(c => c.AppUserId.Equals(appUserId))!.Id;
    }

    protected override IQueryable<Customer> CreateQuery(bool noTracking = true, bool noIncludes = false, bool showDeleted = false)
    {
        var query = base.CreateQuery(noTracking, noIncludes, showDeleted);
        if (noTracking) query = query.AsNoTracking();
        if (!noIncludes)
            query = query.Include(a => a.AppUser)
                .Include(a => a.DisabilityType).ThenInclude(a => a!.DisabilityTypeName)
                .ThenInclude(a => a.Translations);
        return query;
    }
}