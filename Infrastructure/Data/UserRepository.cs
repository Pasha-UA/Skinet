using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserRepository(
            AppIdentityDbContext identityDbContext,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityDbContext = identityDbContext;
        }

        public IReadOnlyList<AppUser> GetUsersAsync()
        {
            var users = _identityDbContext.Users.ToList();
            return users;
        }
        public IReadOnlyList<IdentityUserRole<string>> GetUserRolesAsync()
        {
            var userRoles = _identityDbContext.UserRoles.ToList();
            return userRoles;
        }
        public IReadOnlyList<AppRole> GetRolesAsync()
        {
            var roles = _identityDbContext.Roles.ToList();
            return roles;
        }

        public bool EmailConfirmationRequired(AppUser user)
        {
            if (!_signInManager.Options.SignIn.RequireConfirmedEmail) return false;
            else if (user.EmailConfirmed) return false;
            return true;
        }

        public IEnumerable<AppRole> GetRolesForUser(AppUser user)
        {
            return GetUserRolesAsync().Where(x => x.UserId == user.Id)
                .Join(GetRolesAsync(),ur=>ur.RoleId, r=>r.Id, (ur,r) => r);
        }

        // public IEnumerable<AppUser> GetUsersForRole(AppRole role)
        // {
        //     throw new NotImplementedException();
        // }

        public IEnumerable<AppUser> GetUsersForRole(AppRole role)
        {
            return GetUserRolesAsync().Where(x => x.RoleId == role.Id)
                .Join(GetUsersAsync(),ur=>ur.UserId, u=>u.Id, (ur,u) => u);
        }
        
    }
}