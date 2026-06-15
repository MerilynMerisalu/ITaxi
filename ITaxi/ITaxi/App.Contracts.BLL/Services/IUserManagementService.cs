using App.BLL.DTO.AdminArea;
using App.Domain.Identity;
using Base.Contracts.BLL;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace App.Contracts.BLL.Services 
{
    public interface IUserManagementService
    {
        Task<IEnumerable<UserManagementDTO>>GetUsersAsync(bool noTracking = false);
        Task<UserManagementDTO?> GetUserByIdAsync(Guid id, bool noTracking = false, bool noIncludes = false, bool showDeleted = false, bool showIgnored = false);
        Task<List<UserManagementDTO>> CreateUserManagementDTOAsync(List<AppUser> users);
    }
        
}
