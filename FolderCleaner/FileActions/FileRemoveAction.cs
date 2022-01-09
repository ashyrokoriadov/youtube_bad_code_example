using FolderCleaner.Interfaces;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace FolderCleaner.FileActions
{
    class FileRemoveAction : IFileAction
    {
        private readonly IFileSystem _fileSystem;

        public FileRemoveAction(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void Execute(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                _fileSystem.File.Delete(path);
            }
        }
    }
}
