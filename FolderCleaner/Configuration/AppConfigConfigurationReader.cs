using FolderCleaner.Interfaces;
using System;
using System.Configuration;

namespace FolderCleaner.Configuration
{
    class AppConfigConfigurationReader : IConfigurationReader
    {
        public AppConfiguration Read()
        {
            var configuration = new AppConfiguration();

            configuration.FolderToClean = ConfigurationManager.AppSettings["folderToClean"];
            configuration.Action = ConfigurationManager.AppSettings["action"];
            configuration.Pattern = ConfigurationManager.AppSettings["pattern"];

            var isParsingSuccessful 
                = bool.TryParse(
                    ConfigurationManager.AppSettings["cleanAllFolders"], 
                    out bool cleanAllFolders);

            if (isParsingSuccessful)
                configuration.CleanAllFolders = cleanAllFolders;
            else
                configuration.CleanAllFolders = false;

            Console.WriteLine("Прочитаны настройки приложения");
            Console.WriteLine($"folderToClean = {configuration.FolderToClean}");
            Console.WriteLine($"action = {configuration.Action}");
            Console.WriteLine($"pattern = {configuration.Pattern}");
            Console.WriteLine($"cleanAllFolders = {configuration.CleanAllFolders}");
            Console.WriteLine();

            return configuration;
        }
    }
}
