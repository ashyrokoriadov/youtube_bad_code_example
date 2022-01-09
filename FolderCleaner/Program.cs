using Autofac;
using FolderCleaner.Configuration;
using FolderCleaner.Constants;
using FolderCleaner.FileActions;
using FolderCleaner.Interfaces;
using FolderCleaner.IoC;
using System;
using System.IO.Abstractions;

namespace FolderCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            RegisterDependencies();

            using (var scope = ContainerPreparer.Container.BeginLifetimeScope())
            {
                var fileSelector = scope.Resolve<IFileSelector>();
                var files = fileSelector.Select();

                var actionPerformer = scope.Resolve<IActionPerformer>();
                actionPerformer.Perform(files);

                Console.WriteLine("OK");
                Console.ReadKey();
            }
        }

        private static void RegisterDependencies()
        {
            Console.WriteLine("Начало регистрации зависимостей...");

            ContainerPreparer.Builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            ContainerPreparer.Builder.RegisterType<AppConfigConfigurationReader>().As<IConfigurationReader>().SingleInstance();
            ContainerPreparer.Builder.RegisterType<FileSelector>().As<IFileSelector>().SingleInstance();
            ContainerPreparer.Builder.RegisterType<FileActionFactory>().As<IFileActionFactory>().SingleInstance();
            ContainerPreparer.Builder.RegisterType<ActionPerformer>().As<IActionPerformer>().SingleInstance();

            ContainerPreparer.Builder
                .RegisterType<FileCompressAction>()
                .Named<IFileAction>(Actions.COMPRESS)
                .SingleInstance();

            ContainerPreparer.Builder
                .RegisterType<FileRemoveAction>()
                .Named<IFileAction>(Actions.DELETE)
                .SingleInstance();

            Console.WriteLine("Зависимости зарегистрированы.");
            Console.WriteLine();
        }
    }
}
