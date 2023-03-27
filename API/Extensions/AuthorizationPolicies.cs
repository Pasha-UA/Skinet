using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class AuthorizationPolicies
    {
        // All authorization policies
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                var adminRoles = new string[] { "Admin", "SuperAdmin" };
                options.AddPolicy("AdminOnly",
                    policy => policy.RequireRole(roles: adminRoles));
                options.AddPolicy("ManagerOnly",
                    policy => policy.RequireRole("Manager"));
                options.AddPolicy("AdminRoleClaim", policy =>
                   policy.RequireClaim("Role", "Admin"));
                options.AddPolicy("PartnerRoleClaim", policy =>
                   policy.RequireClaim("Role", "Partner"));
                //                options.FallbackPolicy();
            });

            return services;
        }
    }
}