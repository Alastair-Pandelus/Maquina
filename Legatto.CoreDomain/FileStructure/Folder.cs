using System;
using System.Collections.Generic;
using System.Linq;

namespace Legatto.CoreDomain
{
    abstract public class Folder : FileStructureEntity
    {
        public abstract bool IsRoot();

        public List<FileStructureEntity> Contents { get; set; }

        // User can see this directory downwards in the hierarchy (unless overrriden in sub-directory)
        public List<User> AuthorisedUsers { get; set; }

        public override bool IsAccessible(User user)
        {
            return user.Role == UserRole.Owner || AuthorisedUsers.Contains(user);
        }

        public override int GetFileCount(User user)
        {
            return Contents.Sum(x => x.IsAccessible(user) ? x.GetFileCount(user) : 0);
        }
    }
}
