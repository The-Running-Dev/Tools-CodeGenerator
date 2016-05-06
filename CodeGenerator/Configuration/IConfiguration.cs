namespace CodeGenerator.Configuration
{
    public interface IConfiguration
    {
        string GetConnectionString(string name);
    }
}