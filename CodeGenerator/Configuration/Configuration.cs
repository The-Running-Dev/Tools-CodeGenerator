using System.Configuration;

namespace CodeGenerator.Configuration
{
    public class Configuration : IConfiguration
    {
        public string GetConnectionString(string name)
        {
            var connectionStringNode = ConfigurationManager.ConnectionStrings[name];

            return connectionStringNode.ConnectionString ?? string.Empty;
        }
    }
}