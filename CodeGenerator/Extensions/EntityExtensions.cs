using CodeGenerator.Models;

namespace CodeGenerator.Extensions
{
    public static class EntityExtensions
    {
        public static Repository TotRepository(this Template template, Table table)
        {
            Repository repository;

            if (template.Entity is Repository)
            {
                var r = (Repository)template.Entity;

                repository = new Repository()
                {
                    Namespace = r.Namespace.IsNotEmpty() ? r.Namespace : template.Namespace,
                    Name = string.Format(r.NameFormat,  string.Empty),
                    TableName = table.TableName,
                    ModelName = string.Format(r.NameFormat, string.Empty),
                    ImplementsInterface = r.ImplementsInterface
                };
            }
            else
            {
                repository = new Repository()
                {
                    Namespace = template.Namespace,
                    //Name = $"I{tableName}Repository",
                    TableName = table.TableName,
                    //ModelName = tableName,
                    //ImplementsInterface = "IRepository"
                };
            }

            return repository;
        }
    }
}