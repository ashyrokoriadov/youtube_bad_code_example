using Autofac;
using FolderCleaner.Constants;
using FolderCleaner.Interfaces;
using FolderCleaner.IoC;
using System;

namespace FolderCleaner
{
    class FileActionFactory : IFileActionFactory
    {
        public IFileAction Order(string action)
        {
            using (var scope = ContainerPreparer.Container.BeginLifetimeScope())
            {
                switch (action)
                {
                    case "Delete":
                        Console.WriteLine($"Выбрано действие {Actions.DELETE}");
                        return scope.ResolveNamed<IFileAction>(Actions.DELETE);                        
                    case "Compress":
                        Console.WriteLine($"Выбрано действие {Actions.COMPRESS}");
                        return scope.ResolveNamed<IFileAction>(Actions.COMPRESS);
                    default:
                        throw new Exception("Действие не поддерживается.");
                }
            }
        }
    }
}
