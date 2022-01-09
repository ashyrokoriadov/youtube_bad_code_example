using System.Collections.Generic;

namespace FolderCleaner.Interfaces
{
    interface IFileSelector
    {
        IEnumerable<string> Select();
    }
}
