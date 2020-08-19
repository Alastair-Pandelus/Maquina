using System;
using System.Collections.Generic;
using System.Text;

namespace Legatto.CoreDomain
{
    public abstract class FileStructureEntity
    {
        public abstract int GetFileCount(User user);

        public abstract bool IsAccessible(User user);
    }
}
