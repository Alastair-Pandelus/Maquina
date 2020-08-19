using System;
using System.Collections.Generic;
using System.Text;

namespace Legatto.CoreDomain
{
    public class File : FileStructureEntity
    {
        public string Name { get; set; }

        // Assume just simple text files for sake of example 
        public string Text { get; set; }

        public override int GetFileCount(User user)
        {
            // File access controlled at parent folder level, so just self report atomic existent
            return 1;
        }

        public override bool IsAccessible(User user)
        {
            // Access controlled at directory level (in this example)
            return true;
        }
    }
}
