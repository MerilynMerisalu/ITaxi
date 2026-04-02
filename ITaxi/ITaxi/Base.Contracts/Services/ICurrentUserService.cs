using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Contracts.Services
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? UserEmail { get; }
        string? UserName { get; }
        bool IsAuthenicated { get; }
        bool IsInRole(string roleName);
        IEnumerable<string> RoleNames { get; }
      
    }
}
