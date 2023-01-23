using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using AutoMapper;
using API.Dtos;
using Microsoft.Extensions.Configuration;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace API.Helpers
{
    public class UserRolesResolver : IValueResolver<AppUser, UserDto, IEnumerable<string>>
    {
        private readonly Microsoft.Extensions.Configuration.IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserRolesResolver(
            Microsoft.Extensions.Configuration.IConfiguration config, 
            UserManager<AppUser> userManager, 
            RoleManager<AppRole> roleManager)
        {
            _config = config;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // public async Task<IEnumerable<string>> Resolve(AppUser source, UserDto destination, string destMember, ResolutionContext context)
        // {
        //     var userRoles = await _userManager.GetRolesAsync(source);
        //     if (userRoles!=null)
        //     {
        //         return userRoles;
        //     }

        //     return null;
        // }

        public IEnumerable<string> Resolve(AppUser source, UserDto destination, IEnumerable<string> destMember, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}