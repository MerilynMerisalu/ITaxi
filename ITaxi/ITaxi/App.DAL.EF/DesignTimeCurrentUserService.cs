using Base.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.DAL.EF
{
    public class DesignTimeCurrentUserService : ICurrentUserService
    {
        public Guid? UserId => null;

        public string? UserEmail => "System";

        public string? UserName => "System";

        public bool IsAuthenicated => false;

        public IEnumerable<string> RoleNames => Enumerable.Empty<string>();

        public bool IsInRole(string roleName) => false;
    }
}
