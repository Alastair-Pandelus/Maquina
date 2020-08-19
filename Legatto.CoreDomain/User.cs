using System;
using System.Collections.Generic;
using System.Text;

namespace Legatto.CoreDomain
{
    public class User
    {   
        public UserRole Role { get; set; }

        public string Name { get; set; }
        
        public string Username { get; set; }

        public string Email { get; set; }
    }
}
