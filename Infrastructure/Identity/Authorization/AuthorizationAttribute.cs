using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.Identity.Authorization
{

// for debugging authorization. not helped...
    public class AuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _roles;
        public AuthorizationAttribute(string Roles)
        {
            _roles = Roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
           return;
        }
    }
}