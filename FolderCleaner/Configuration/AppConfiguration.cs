namespace FolderCleaner.Configuration
{
    class AppConfiguration
    {
        public string FolderToClean { get; set; }

        public string Action { get; set; }

        public string Pattern { get; set; }

        public bool CleanAllFolders { get; set; } 
    }
}
