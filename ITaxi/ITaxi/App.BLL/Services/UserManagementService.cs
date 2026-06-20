using App.BLL.DTO.AdminArea;

using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using App.Contracts.BLL.Services;
using App.Contracts.BLL;

namespace App.BLL.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IAppBLL _appBll;
        
        public UserManagementService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IAppBLL appBll)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appBll = appBll;
        }

        public async Task<List<UserManagementDTO>> CreateUserManagementDTOAsync(List<AppUser> users)
        {
            var result = new List<UserManagementDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var displayNames = await _roleManager.Roles.Include(r => r.DisplayName).ThenInclude(r => r.Translations)
                .Where(role => roles.Contains(role.Name!))
                .Select(role => role.DisplayName)
                .ToListAsync();
                var data = new UserManagementDTO()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender!.Value,
                    DateOfBirth = user.DateOfBirth.ToShortDateString(),
                    Role = roles.Any() ? string.Join(", ", roles) : "-",
                    EmailAddress = user.Email!,
                    PhoneNumber = user.PhoneNumber!
                };
                var admin = await _appBll.Admins.GetAdminByAppUserIdAsync(user.Id);
                if (admin != null)
                {
                  data.PersonalIdentifier = admin.PersonalIdentifier;
                  data.Country = admin.City.County.Country.CountryName;
                  data.County = admin.City.County.CountyName;
                  data.City = admin.City.CityName;
                  data.AddressOfResidence = admin.Address;
                    
                }

                result.Add(data);

            }
            return result;
        }
        

        public async Task<UserManagementDTO?> GetUserByIdAsync(Guid id, bool noTracking = false, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
        {
            var users = new List<AppUser>();
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user != null)
            {
                users.Add(user);
                var result = await CreateUserManagementDTOAsync(users);
                return result[0];
            }
            return null;
        }

        public async Task<IEnumerable<UserManagementDTO>> GetUsersAsync(bool noTracking = false)
        {
            var query = _userManager.Users.AsQueryable();
            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            var users = await query.ToListAsync();
            var result = await CreateUserManagementDTOAsync(users);
            return result;
        }

    }
}
