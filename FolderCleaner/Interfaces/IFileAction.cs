using System.Collections.Generic;

namespace FolderCleaner.Interfaces
{
    interface IFileAction
    {
        void Execute(IEnumerable<string> paths);
    }
}
