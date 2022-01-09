using System.Collections.Generic;

namespace FolderCleaner.Interfaces
{
    interface IActionPerformer
    {
        void Perform(IEnumerable<string> paths);
    }
}
