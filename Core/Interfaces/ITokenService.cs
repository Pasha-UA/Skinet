using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities.Identity;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken (AppUser user, bool rememberMe);
        ClaimsPrincipal ValidateToken(string jwtToken);
    }
}