using StructureMap;

using CodeGenerator.Bootstraper.Registries;

namespace CodeGenerator.Bootstraper
{
    public class Bootstrapper
    {
        public static IIocWrapper Bootstrap(BootstrapType type)
        {
            var container = new Container();

            container.Configure(cfg => cfg.For<IIocWrapper>().Use(IocWrapper.Instance));

            switch (type)
            {
                case BootstrapType.Web:
                    ConfigureWebRegistries(container);

                    break;
                case BootstrapType.Api:
                    ConfigureApiRegistries(container);

                    break;
                case BootstrapType.IntegrationTest:
                    ConfigureIntegrationTestRegistries(container);

                    break;
            }

            IocWrapper.Instance = new IocWrapper(container);

            return IocWrapper.Instance;
        }

        public static void ConfigureWebRegistries(IContainer container)
        {
            container.Configure(cfg =>
            {
                cfg.AddRegistry<FrameworkRegistry>();
                cfg.AddRegistry<DataRegistry>();
            });
        }

        public static void ConfigureApiRegistries(IContainer container)
        {
            container.Configure(cfg =>
            {
                cfg.AddRegistry<FrameworkRegistry>();
                cfg.AddRegistry<DataRegistry>();
            });
        }

        public static void ConfigureIntegrationTestRegistries(IContainer container)
        {
            container.Configure(cfg =>
            {
                cfg.AddRegistry<DataRegistry>();
            });
        }
    }
}