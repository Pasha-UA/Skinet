using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class AppUserRole : IdentityUserRole<string>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}