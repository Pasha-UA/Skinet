using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"(?=^.{6,10}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\s).*$",
        ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphabetic and at least 6 characters")
        ]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"(^[0]+[0-9]{9}$)", ErrorMessage = @"Phone number starts with '0' and contains digits only")]
        public string PhoneNumber { get; set; }

        public bool RememberMe { get; set; }
    }
}