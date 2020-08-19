using System;
using System.Collections.Generic;

namespace Legatto.CoreDomain
{
    public class Account
    {
        public int Id { get; set; }

        public User Owner { get; set; }

        public List<User> AccreditedParties { get; set; }

        // All users = owner + accredited parties
        public List<User> Users
        {
            get
            {
                List<User> users = new List<User> { Owner };
                users.AddRange(AccreditedParties);

                return users;
            }
        }

        public RootFolder RootFolder { get; set; }

        public int GetFileCount(User user)
        {
            return RootFolder.GetFileCount(user);
        }
    }
}
