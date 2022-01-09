using Autofac;

namespace FolderCleaner.IoC
{
    static class ContainerPreparer
    {
        private static IContainer _container;
        public static IContainer Container => _container ?? (_container = Builder.Build());

        private static ContainerBuilder _builder;
        public static ContainerBuilder Builder => _builder ?? (_builder = new ContainerBuilder());
    }
}
