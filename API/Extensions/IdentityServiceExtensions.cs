using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
//             var builder = services.AddIdentity<AppUser, AppRole>(options=> 
//             {
//                 options.Password.RequiredLength = 8;
//                 options.Password.RequireUppercase = true;
//                 options.Password.RequireLowercase = true;

//                 options.Lockout.MaxFailedAccessAttempts = 5;
//                 options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

//                 options.User.RequireUniqueEmail = true;
// //                options.SignIn.RequireConfirmedEmail = true;

//             });
//             builder = new IdentityBuilder(builder.UserType, typeof(AppRole), builder.Services);
//             builder.AddEntityFrameworkStores<AppIdentityDbContext>();
//             builder.AddSignInManager<SignInManager<AppUser>>();
//             builder.AddRoleValidator<RoleValidator<AppRole>>();
//             builder.AddRoleManager<RoleManager<AppRole>>();
//             builder.AddUserManager<UserManager<AppUser>>();

            services.AddIdentity<AppUser, AppRole>(options=> 
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                options.User.RequireUniqueEmail = true;
//                options.SignIn.RequireConfirmedEmail = true;

            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoleValidator<RoleValidator<AppRole>>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddUserManager<UserManager<AppUser>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                        ValidIssuer = config["Token:Issuer"],
                        ValidateIssuer = true,
                        ValidateAudience = false

                    };
                })
                .AddCookie(IdentityConstants.TwoFactorUserIdScheme, options =>
                {
                   options.Cookie.Name = "Skinet";
                   options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                })
                ;
//                        services.AddAuthorization(options=>options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireRole());



            services.AddAuthorization(options =>
            {
                var AdminRoles = new string[] { "Admin", "Manager" };
                options.AddPolicy("AdminOnly",
                    policy => policy.RequireRole(roles: AdminRoles));
                options.AddPolicy("ManagerOnly",
                    policy => policy.RequireRole("Manager"));
                 options.AddPolicy("AdminRoleClaim", policy => 
                    policy.RequireClaim("Role", "Admin"));  
            });


            //            services.AddAuthorization(options=>options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireRole().);

            return services;
        }
    }
}