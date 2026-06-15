using App.BLL.DTO.AdminArea;

using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using App.Contracts.BLL.Services;

namespace App.BLL.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public UserManagementService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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

                result.Add(new UserManagementDTO
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = roles.Any() ? String.Join(", ", displayNames) : "-",
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,

                });


            }
            return result;
        }
        

        public async Task<UserManagementDTO?> GetUserByIdAsync(Guid id, bool noTracking = false, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false)
        {
            throw new NotImplementedException();
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
