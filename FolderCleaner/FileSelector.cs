using FolderCleaner.Configuration;
using FolderCleaner.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace FolderCleaner
{
    class FileSelector : IFileSelector
    {
        private readonly IFileSystem _fileSystem;
        private readonly AppConfiguration _appConfiguration;
        public FileSelector(IFileSystem fileSystem, IConfigurationReader configurationReader)
        {
            _fileSystem = fileSystem;
            _appConfiguration = configurationReader.Read();
        }

        public IEnumerable<string> Select()
        {
            try
            {
                IEnumerable<string> files;

                if (_appConfiguration.CleanAllFolders)
                {
                    files = _fileSystem.Directory.EnumerateFiles(
                        _appConfiguration.FolderToClean,
                        _appConfiguration.Pattern,
                        SearchOption.AllDirectories);
                }
                else
                {
                    files =_fileSystem.Directory.EnumerateFiles(
                        _appConfiguration.FolderToClean,
                        _appConfiguration.Pattern,
                        SearchOption.TopDirectoryOnly);
                }

                Console.WriteLine($"Прочитаны пути к файлам: {files.ToArray().Length}");

                return files;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
