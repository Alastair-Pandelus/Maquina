using System;

namespace Legatto.CoreDomain
{
    public class RootFolder : Folder
    {
        public override bool IsRoot() => true;
    }
}
