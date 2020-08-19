using System;

namespace Legatto.CoreDomain
{
    public class ChildFolder : Folder
    {
        public override bool IsRoot() => false;

        public string Name { get; set; }
    }
}
