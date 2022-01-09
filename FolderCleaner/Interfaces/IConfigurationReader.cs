using FolderCleaner.Configuration;

namespace FolderCleaner.Interfaces
{
    interface IConfigurationReader
    {
        AppConfiguration Read();
    }
}
