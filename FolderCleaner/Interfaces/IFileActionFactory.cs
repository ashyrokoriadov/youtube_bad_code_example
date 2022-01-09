using FolderCleaner.Interfaces;

namespace FolderCleaner
{
    interface IFileActionFactory
    {
        IFileAction Order(string action);
    }
}
