using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {

        public static async Task SeedRolesAsync(RoleManager<UserRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var roles = new UserRole[]
                {
                        new UserRole { Id = "1", Name = "Admin" },
                        new UserRole { Id = "3", Name = "User" },
                        new UserRole { Id = "2", Name = "Manager" }
                };
                foreach (var role in roles) 
                {
                    await roleManager.CreateAsync(role);                    
                }
            }
        }

        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Bob",
                    Email = "bob@test.com",
                    UserName = "bob@test.com",
                    Address = new Address
                    {
                        FirstName = "Bob",
                        LastName = "Bobbity",
                        Street = "10th Street",
                        City = "New York",
                        State = "NY",
                        ZipCode = "90210"
                    },
                    UserRole = new UserRole { Id = "1", Name = "Admin" }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}