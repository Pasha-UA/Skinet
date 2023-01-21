using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;

namespace Core.Specifications
{
    public class UsersSpecification : BaseSpecification<AppUser>
    {
        public UsersSpecification() : base()
        {
            AddInclude(o => o.Email);
            AddInclude(o => o.DisplayName);
            AddInclude(o => o.PhoneNumber);
            AddInclude(o => o.EmailConfirmed);
            AddInclude(o => o.Address);
// add role
        }

    }
}