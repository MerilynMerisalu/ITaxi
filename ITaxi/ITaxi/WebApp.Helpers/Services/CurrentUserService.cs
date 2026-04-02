using Base.Contracts.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Claims;
using System.Text;
using Base.Extensions;

namespace WebApp.Helpers.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
        public Guid? UserId => GetAuthenticatedUser()?.GetUserId();
        
            
         private ClaimsPrincipal? GetAuthenticatedUser()
        {
            if (!IsAuthenicated || User == null)
            {
                return null;
            }
            return User;
        }
            
       
        public string? UserName => GetAuthenticatedUser()?.GetUserName();

        public bool IsAuthenicated => User?.Identity?.IsAuthenticated ?? false;

        public string? UserEmail => GetAuthenticatedUser()?.GetUserEmail();

        public IEnumerable<string> RoleNames => GetAuthenticatedUser()?.GetUserRoleNames() ?? Enumerable.Empty<string>();

        public bool IsInRole(string roleName)
        {
            return GetAuthenticatedUser()?
                .IsInRole(roleName) ?? false;
        }
    }
}
