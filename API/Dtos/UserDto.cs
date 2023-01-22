using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;

namespace API.Dtos
{
    public class UserDto
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber {get; set; }
 //       public Address Address { get; set; }
        public string Token { get; set; }
        public bool EmailConfirmationRequired { get; set; } = false;
        public bool PhoneConfirmationRequired { get; set; } = false;
        public bool AccountConfirmationRequired { get; set; } = false;
        public bool AccountLocked { get; set; } = false;
        public IEnumerable<AppRole> Roles { get; set; }

        public bool RememberMe {get; set;}

    }
}