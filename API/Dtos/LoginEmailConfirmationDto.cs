using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class LoginEmailConfirmationDto
    {
        public string Email { get; set; }
        public string ConfirmationCode { get; set; }
        public bool RememberMe {get; set;} = true;
    }
}