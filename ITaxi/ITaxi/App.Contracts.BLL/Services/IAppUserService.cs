using App.Contracts.DAL.IAppRepositories;
using App.DAL.DTO.Identity;
using App.Domain.Identity;
using App.Enum.Enum;
using Base.Contracts.BLL;
using AppUser = App.BLL.DTO.Identity.AppUser;


namespace App.Contracts.BLL.Services;

public interface IAppUserService : IEntityService<App.BLL.DTO.Identity.AppUser>,
    IAppUserRepositoryCustom<App.BLL.DTO.Identity.AppUser>
{
    Task<AppUser> GettingAppUserByAppUserIdAsync(Guid appUserId, bool noTracking = true,
        bool noIncludes = false);
    bool ValidateUsersDateOfBirth(DateTime dateOfBirth);
    bool ValidateAge(DateTime dateOfBirth);
    bool ValidateUsersGender(Gender choosedGender, int genderFromPersonalIdentifierCode);
    bool ValidateUsersChosenDateOfBirth(DateOnly chosenDateOfBirth, DateOnly dateOfBirthFromPersonalIdentifierCode);

}