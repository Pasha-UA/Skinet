using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public Address Address { get; set; }

//        public bool RememberMe {get; set;} 
//        [Required]
//        public AppRole AppRole { get; set; }

    }
}