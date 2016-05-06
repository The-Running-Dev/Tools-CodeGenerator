using StructureMap;
using StructureMap.Graph;
using StructureMap.Graph.Scanning;

using CodeGenerator.Providers;
using CodeGenerator.SqlDialects;

namespace CodeGenerator.Bootstraper.Registries
{
    public class DataRegistry : Registry
    {
        public DataRegistry()
        {
            Scan(scan =>
            {
                // Scan all the assemblies with the default fault convention
                scan.AssembliesFromApplicationBaseDirectory();

                // The default conversion is IService being implemented by Service
                scan.WithDefaultConventions();
            });

            For<IDatabaseProvider>().Use<MySqlProvider>();
            For<ISqlDialect>().Use<MySqlDialect>();

            //For<ICrsConnection>().Add<CrsConnection>()
            //    .Ctor<IConfiguration>().Is<MyCodeConfiguration>()
            //    .Ctor<String>().Is("CRS_Local");

            //For(typeof(IConfiguration)).Singleton().Use(typeof(MyCodeConfiguration));

            //For<ICrsConnection>().Add<CrsConnection>().Ctor<String>().Is("CRS_Local");
        }
    }

    public class DataAssemblyScanner : DefaultConventionScanner
    {
        public override void ScanTypes(TypeSet type, Registry registry)
        {
            base.ScanTypes(type, registry);
        }
    }
}