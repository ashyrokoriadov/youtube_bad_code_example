using FolderCleaner.Configuration;
using FolderCleaner.Interfaces;
using System.Collections.Generic;

namespace FolderCleaner
{
    class ActionPerformer : IActionPerformer
    {
        private readonly IFileActionFactory _fileActionFactory;
        private readonly AppConfiguration _appConfiguration;
        public ActionPerformer(IFileActionFactory fileActionFactory, IConfigurationReader configurationReader)
        {
            _fileActionFactory = fileActionFactory;
            _appConfiguration = configurationReader.Read();
        }

        public void Perform(IEnumerable<string> paths)
        {
            var fileAction = _fileActionFactory.Order(_appConfiguration.Action);
            fileAction.Execute(paths);
        }
    }
}
