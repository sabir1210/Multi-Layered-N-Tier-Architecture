using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTier.DataObject.DTOs.User
{
    // Models/UserRegistrationDTO.cs
    public class UserRegistrationDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    // Models/UserLoginDTO.cs
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
