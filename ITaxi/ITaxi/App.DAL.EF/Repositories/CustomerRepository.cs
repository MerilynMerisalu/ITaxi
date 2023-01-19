/*using App.Contracts.DAL.IAppRepositories;
using App.Domain;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CustomerRepository : BaseEntityRepository<Customer, AppDbContext>, ICustomerRepository
{
    public CustomerRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<IEnumerable<Customer>> GetAllAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).ToListAsync();
    }

    public override IEnumerable<Customer> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList();
    }

    public override async Task<Customer?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return await CreateQuery(noTracking).FirstOrDefaultAsync(c => c.Id.Equals(id));
    }

    public override Customer? FirstOrDefault(Guid id, bool noTracking = true)
    {
        return CreateQuery(noTracking).FirstOrDefault(c => c.Id.Equals(id));
    }

    public async Task<IEnumerable<Customer?>> GettingAllCustomersWithoutIncludesAsync(bool noTracking = true)
    {
        return await base.CreateQuery(noTracking, noIncludes: true)
            .ToListAsync();
    }

    public IEnumerable<Customer?> GettingAllCustomersWithoutIncludes(bool noTracking = true)
    {
        return base.CreateQuery(noTracking, noIncludes: true).ToList();
    }

    public async Task<IEnumerable<Customer?>> GettingAllOrderedCustomersWithoutIncludesAsync(bool noTracking = true)
    {
        return await base.CreateQuery(noTracking, noIncludes: true).OrderBy(c => c.DisabilityType).ToListAsync();
    }

    public async Task<Customer?> GettingCustomerByIdWithoutIncludesAsync(Guid id, bool noTracking = true)
    {
        return await base.CreateQuery(noTracking, noIncludes: true).FirstOrDefaultAsync(c => c.Id.Equals(id));
    }

    public async Task<IEnumerable<Customer?>> GettingAllOrderedCustomersAsync(bool noTracking = true)
    {
        return await CreateQuery(noTracking).OrderBy(c => c.AppUser!.LastName)
            .ThenBy(c => c.AppUser!.FirstName).ToListAsync();
    }

    public IEnumerable<Customer?> GettingAllOrderedCustomers(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(c => c.AppUser!.LastName)
            .ThenBy(c => c.AppUser!.FirstName).ToList();
    }

    public Customer? GettingCustomerByIdWithoutIncludes(Guid id, bool noTracking = true)
    {
        return base.CreateQuery(noTracking, noIncludes: true).FirstOrDefault(c => c.Id.Equals(id));
    }

    protected override IQueryable<Customer> CreateQuery(bool noTracking = true, bool noIncludes = false)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();
        if (!noIncludes)
            query = query.Include(a => a.AppUser)
                .Include(a => a.DisabilityType).ThenInclude(a => a!.DisabilityTypeName)
                .ThenInclude(a => a.Translations);
        return query;
    }
}*/