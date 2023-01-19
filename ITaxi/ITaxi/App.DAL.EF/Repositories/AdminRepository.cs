using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.AdminArea;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class AdminRepository : BaseEntityRepository<App.DAL.DTO.AdminArea.AdminDTO, App.Domain.Admin, AppDbContext>, IAdminRepository
{
    public AdminRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.AdminArea.AdminDTO, App.Domain.Admin> mapper)
        : base(dbContext, mapper)
    {
    }

    public override IEnumerable<AdminDTO> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(e => Mapper.Map(e))!;
    }

    public override async Task<IEnumerable<AdminDTO>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public override async Task<AdminDTO?> FirstOrDefaultAsync(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await CreateQuery(noTracking, noIncludes).FirstOrDefaultAsync(e => e.Id.Equals(id)));
    }

    public override AdminDTO? FirstOrDefault(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(CreateQuery(noTracking, noIncludes).FirstOrDefault(a => a.Id.Equals(id)));
    }

    public async Task<IEnumerable<AdminDTO>> GetAllAdminsOrderedByLastNameAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).OrderBy(a => a.AppUser!.LastName)
            .ThenBy(a => a.AppUser!.FirstName).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<AdminDTO> GetAllAdminsOrderedByLastName(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(a => a.AppUser!.LastName)
            .ThenBy(a => a.AppUser!.FirstName).ToList().Select(e => Mapper.Map(e))!;
    }

    protected override IQueryable<Admin> CreateQuery(bool noTracking = true, bool noIncludes = false)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();
        if (!noIncludes)
            query = query.Include(a => a.AppUser)
                         .Include(a => a.City);
        return query;
    }
}