
using App.BLL.DTO.Identity;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.IAppRepositories;
using Base.BLL;
using Base.Contracts.Mappers;

namespace App.BLL.Services;

public class AppUserService : BaseEntityService<App.BLL.DTO.Identity.AppUser, App.DAL.DTO.Identity.AppUser, IAppUserRepository>,
    IAppUserService
{
    public AppUserService(IAppUserRepository repository, IMapper<AppUser, DAL.DTO.Identity.AppUser> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<AppUser>> GetAllAppUsersOrderedByLastNameAsync(bool noTracking = true)
    {
        return (await Repository.GetAllAppUsersOrderedByLastNameAsync(noTracking)).Select(e => Mapper.Map(e))!;
    }

    public IEnumerable<AppUser> GetAllAppUsersOrderedByLastName(bool noTracking = true)
    {
        return Repository.GetAllAppUsersOrderedByLastName(noTracking).Select(e => Mapper.Map(e))!;
    }

    public async Task<AppUser> GettingAppUserByAppUserIdAsync(Guid appUserId, bool noTracking = true, bool noIncludes = false)
    {
        return Mapper.Map(await Repository.FirstOrDefaultAsync(appUserId, noTracking, noIncludes))!;
    }

    public bool ValidateUsersDateOfBirth(DateTime dateOfBirth)
    {
        var dateOfToday = DateTime.Today;
        bool result = dateOfBirth <= dateOfToday;
        return result;
    }

    public bool ValidateAge(DateTime dateOfBirth)
    {
        var dateOfToday = DateTime.Today;
        int age = 0;
        const int MINIMUMREGISTRATIONAGE = 18;
        if ((dateOfBirth.Month > dateOfToday.Month) || (dateOfBirth.Month == dateOfToday.Month && 
            (dateOfBirth.Day > dateOfToday.Day)))
        {
            age = dateOfToday.Year - dateOfBirth.Year - 1;
        }
        else
        {
            age = dateOfToday.Year - dateOfBirth.Year;
        }

        if (age < MINIMUMREGISTRATIONAGE)
            return false;
        return true;
      

    }
}