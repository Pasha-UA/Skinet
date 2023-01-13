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

        public static async Task SeedRolesAsync(RoleManager<AppRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                var roles = new AppRole[]
                {
                        new AppRole { Id = "1", Name = "Admin" },
                        new AppRole { Id = "3", Name = "User" },
                        new AppRole { Id = "2", Name = "Manager" }
                };
                foreach (var role in roles)
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }

        public static async Task SeedUsersAsync(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
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
                        }
                    },
                    new AppUser
                    {
                        DisplayName = "Admin",
                        Email = "admin@test.com",
                        UserName = "admin@test.com"
                    }


                };

                await SeedRolesAsync(roleManager);


                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(user, "User");
                    if (user.Email == "admin@test.com") await userManager.AddToRoleAsync(user, "Admin");
                };

            }
        }
    }
}