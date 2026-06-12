using App.BLL.DTO.AdminArea;

using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace App.Contracts.BLL.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<App.Domain.Identity.AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public UserManagementService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IEnumerable<UserManagementDTO>> GetUsersAsync(bool noTracking = false)
        {
            var query = _userManager.Users.AsQueryable();
            if (noTracking)
            {
                query = query.AsNoTracking();
            }

            var users = await query.ToListAsync();
            var result = new List<UserManagementDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserManagementDTO
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = roles.Any() ? String.Join(", ", roles) : "-",
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,

                });

               
            }
            return result;
        }

    }
}
