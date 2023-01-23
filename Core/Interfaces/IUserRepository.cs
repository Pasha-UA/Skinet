using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        IReadOnlyList<AppUser> GetUsersAsync();
        IReadOnlyList<IdentityUserRole<string>> GetUserRolesAsync();
        IReadOnlyList<AppRole> GetRolesAsync();
        bool EmailConfirmationRequired(AppUser user);
        IEnumerable<AppRole> GetRolesForUser(AppUser user);
        IEnumerable<AppUser> GetUsersForRole(AppRole role);
    }
}