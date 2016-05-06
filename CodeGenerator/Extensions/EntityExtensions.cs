using CodeGenerator.Models;

namespace CodeGenerator.Extensions
{
    public static class EntityExtensions
    {
        public static Repository ToRepository(this Template template, Table table)
        {
            Repository repository;

            if (template.Entity is Repository)
            {
                var r = (Repository)template.Entity;

                repository = new Repository()
                {
                    Namespace = r.Namespace.IsNotEmpty() ? r.Namespace : template.Namespace,
                    Name = r.Formatter.Format(r.Name, table),
                    TableName = r.Formatter.Format(r.TableName, table),
                    ModelName = r.Formatter.Format(r.ModelName, table),
                    BaseRepository = r.Formatter.Format(r.BaseRepository, table),
                    ImplementsInterface = r.Formatter.Format(r.ImplementsInterface, table),
                    ConnectionInterface = r.Formatter.Format(r.ConnectionInterface, table)
                };
            }
            else
            {
                repository = new Repository()
                {
                    Namespace = template.Namespace,
                    Name = $"I{table.TableName}Repository",
                    TableName = table.TableName.PluralToSingular(),
                    ModelName = table.TableName,
                    ImplementsInterface = "IRepository",
                    ConnectionInterface = "IConnection"
                };
            }

            // Add the "using" directives from the template
            repository.UsingDirectives.AddRange(template.UsingDirectives);

            return repository;
        }
    }
}