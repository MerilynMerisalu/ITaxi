using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.Identity;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class AppUserRepository : BaseEntityRepository<App.DAL.DTO.Identity.AppUser, App.Domain.Identity.AppUser, AppDbContext>, IAppUserRepository
{
    public AppUserRepository(AppDbContext dbContext, IMapper<App.DAL.DTO.Identity.AppUser, App.Domain.Identity.AppUser> mapper)
        : base(dbContext, mapper)
    {
    }

    public override IEnumerable<DTO.Identity.AppUser> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(e => Mapper.Map(e))!;
    }

    public override async Task<IEnumerable<DTO.Identity.AppUser>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public override async Task<DTO.Identity.AppUser?> FirstOrDefaultAsync(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await CreateQuery(noTracking, noIncludes).FirstOrDefaultAsync(e => e.Id.Equals(id)));
    }

    public override DTO.Identity.AppUser? FirstOrDefault(Guid id, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(CreateQuery(noTracking, noIncludes).FirstOrDefault(a => a.Id.Equals(id)));
    }

    public async Task<IEnumerable<DTO.Identity.AppUser>> GetAllAppUsersOrderedByLastNameAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName).ToListAsync()).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<DTO.Identity.AppUser> GetAllAppUsersOrderedByLastName(bool noTracking = true)
    {
        return CreateQuery(noTracking).OrderBy(a => a.LastName)
            .ThenBy(a => a.FirstName).ToList().Select(e => Mapper.Map(e))!;
    }
/*
    protected override IQueryable<Domain.Identity.AppUser> CreateQuery(bool noTracking = true, bool noIncludes = false)
    {
     //   var query = RepoDbSet.AsQueryable();
     //   if (noTracking) query = query.AsNoTracking();
     //   return query;
     return base.CreateQuery(noTracking, noIncludes);
    }
*/
    public override AppUser Update(AppUser entity)
    {
        // Deliberately NOT using Mapper to resolve the domain entity here
        // It is unclear exactly which property is causing trouble, but Mapper is assigning properties that we shouldn'ˇt be, perhaps the password?
        // As a special case for this AppUser Entity, we are using explicit mapping.
        // - AutoMapper with Ignore rules might also do this job.
        
        //var domain = Mapper.Map(entity);
        
        var domain = CreateQuery().FirstOrDefault(x => x.Id == entity.Id)!;
        domain.FirstName = entity.FirstName;
        domain.LastName = entity.LastName;
        domain.Gender = entity.Gender;
        domain.DateOfBirth = entity.DateOfBirth;
        domain.PhoneNumber = entity.PhoneNumber;
        domain.IsActive = entity.IsActive;
        
        var result = RepoDbSet.Update(domain!);
        
        return Mapper.Map(result.Entity)!;
    }


    

}