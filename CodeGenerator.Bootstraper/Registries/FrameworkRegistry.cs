using StructureMap;

namespace CodeGenerator.Bootstraper.Registries
{
    public class FrameworkRegistry : Registry
    {
        public FrameworkRegistry()
        {
            //For<IDatabaseServer>().Use<SqlDatabaseServer>();
            //For<ISerializer>().Use<JsonSerializer>();
            //For<IJsonSerializer>().Use<JsonSerializer>();
            //For<IXmlSerializer>().Use<XmlSerializer>();
            //For(typeof(IConfiguration<>)).Singleton().Use(typeof(MyCodeConfiguration<>));
            //For<IConfiguration>().Singleton().Use<MyCodeConfiguration>();
            //For<ILogService>().Use<Log4NetLogService>().Ctor<string>().Is(string.Empty);
            ////For<HttpContextBase>().Use(() => new HttpContextWrapper(HttpContext.Current));
            //For<IWebUtils>().Use<WebUtils>();
        }
    }
}