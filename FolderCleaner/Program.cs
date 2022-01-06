using Autofac;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Abstractions;
using System.Linq;

namespace FolderCleaner
{
    class Program
    {
        static string folderToClean;
        static string action;
        static string pattern;
        static bool cleanAllFolders = false;
        static IEnumerable<string> files;

        static void Main(string[] args)
        {
            RegisterDependencies();
            ReadConfiguration();
            files = SelectFiles();
            PerformAction();

            Console.WriteLine("OK");
            Console.ReadKey();
        }

        private static void RegisterDependencies()
        {
            Console.WriteLine("Начало регистрации зависимостей...");
            Builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            Builder.RegisterType<FileRemover>().As<IRemover>().SingleInstance();
            Builder.RegisterType<FileCompressor>().As<ICompressor>().SingleInstance();
            Console.WriteLine("Зависимости зарегистрированы.");
            Console.WriteLine();
        }

        private static void ReadConfiguration()
        {
            folderToClean = ConfigurationManager.AppSettings["folderToClean"];
            action = ConfigurationManager.AppSettings["action"];
            pattern = ConfigurationManager.AppSettings["pattern"];
            bool.TryParse(ConfigurationManager.AppSettings["cleanAllFolders"], out cleanAllFolders);

            Console.WriteLine("Прочитаны настройки приложения");
            Console.WriteLine($"folderToClean = {folderToClean}");
            Console.WriteLine($"action = {action}");
            Console.WriteLine($"pattern = {pattern}");
            Console.WriteLine($"cleanAllFolders = {cleanAllFolders}");
            Console.WriteLine();
        }

        private static IEnumerable<string> SelectFiles()
        {
            using (var scope = Container.BeginLifetimeScope())
            {               
                try
                {
                    var filesystem = scope.Resolve<IFileSystem>();
                    IEnumerable<string> files;

                    if (cleanAllFolders)
                    {
                        files = filesystem.Directory.EnumerateFiles(folderToClean, pattern, SearchOption.AllDirectories);
                    }
                    else
                    {
                        files = filesystem.Directory.EnumerateFiles(folderToClean, pattern, SearchOption.TopDirectoryOnly);
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

        private static void PerformAction()
        {
            using (var scope = Container.BeginLifetimeScope())
            {
                switch (action)
                {
                    case "Delete":
                        Console.WriteLine($"Выбрано действие {Actions.DELETE}");
                        var fileRemover = scope.Resolve<IRemover>();
                        fileRemover.Remove(files.ToArray());
                        break;
                    case "Compress":
                        Console.WriteLine($"Выбрано действие {Actions.COMPRESS}");
                        var fileCompressor = scope.Resolve<ICompressor>();
                        fileCompressor.Compress(files.ToArray());
                        break;
                    default:
                        throw new Exception("Действие не поддерживается.");
                }
            }
        }

        private static IContainer _container;
        public static IContainer Container => _container ?? (_container = Builder.Build());

        private static ContainerBuilder _builder;
        public static ContainerBuilder Builder => _builder ?? (_builder = new ContainerBuilder());
    }

    class FileRemover : IRemover
    {
        private readonly IFileSystem _fileSystem;

        public FileRemover(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public void Remove(string[] paths)
        {
            foreach (var path in paths)
            {
                _fileSystem.File.Delete(path);
            }
        }
    }

    interface IRemover
    {
        void Remove(string[] paths);
    }

    class FileCompressor : ICompressor
    {
        public void Compress(string[] paths)
        {
            foreach (var path in paths)
            {
                var fileName = Path.GetFileNameWithoutExtension(path);
                var directoryName = Path.GetDirectoryName(path);

                using (var outputStream = new ZipOutputStream(File.Create($"{directoryName}\\{fileName}.zip")))
                {
                    outputStream.SetLevel(9);

                    byte[] buffer = new byte[4096];
                    var entry = new ZipEntry(Path.GetFileName(path));
                    entry.DateTime = DateTime.Now;
                    outputStream.PutNextEntry(entry);

                    using (var fileStream = File.OpenRead(path))
                    {
                        int sourceBytes;
                        do
                        {
                            sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                            outputStream.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }

                    outputStream.Finish();
                    outputStream.Close();
                }

                Console.WriteLine("Архив создан.");
            }
        }
    }
    interface ICompressor
    {
        void Compress(string[] paths);
    }

    static class Actions
    {
        public const string DELETE = "Delete";
        public const string COMPRESS = "Compress";
    }
}
