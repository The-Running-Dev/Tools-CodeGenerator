using System.Collections.Generic;

using CodeGenerator.Models;
using CodeGenerator.Providers;
using CodeGenerator.Extensions;
using CodeGenerator.Services.Interfaces;

namespace CodeGenerator
{
    public class MySqlGenerator
    {
        public MySqlGenerator(IDatabaseProvider provider, ITemplateService templateService, IFileService fileService)
        {
            _provider = provider;
            _templateService = templateService;
            _fileService = fileService;
        }

        public List<string> GenerateModels(Template template)
        {
            var generatedFiles = new List<string>();

            // Get all the tables
            var tables = _provider.GetTables(template.Entities);

            foreach (var table in tables)
            {
                // Create an entity for each table
                var entity = new Table()
                {
                    Namespace = template.Namespace,
                    Name = table.TableName.PluralToSingular(),
                    TableName = table.TableName,
                    Columns = table.Columns
                };

                // Add the "using" directives from the template
                entity.UsingDirectives.AddRange(template.UsingDirectives);

                template.FileNameWithoutExtension = entity.Name;

                // Generate the contents based on the entity using the PathToTemplate
                template.Contents = _templateService.Generate(entity, template.PathToTemplate);

                // Write the template to disk if the path is not empty
                if (template.Path.IsNotEmpty())
                {
                    var path = _fileService.Write(template);

                    if (path.IsNotEmpty()) generatedFiles.Add(path);
                }
            }

            return generatedFiles;
        }

        public void GenerateRepositoryInterfaces(Template template)
        {
            // Get all the tables
            var tables = _provider.GetTables(template.Entities);

            foreach (var table in tables)
            {
                // Create an repository entity for each table
                var entity = template.ToRepository(table);

                // Set the file extensions for the template
                template.FileNameWithoutExtension = entity.Name;

                // Generate the contents based on the entity using the PathToTemplate
                template.Contents = _templateService.Generate(entity, template.PathToTemplate);

                // Write the template to disk
                _fileService.Write(template);
            }
        }

        public void GenerateRepositories(Template template)
        {
            // Get all the tables
            var tables = _provider.GetTables(template.Entities);

            foreach (var table in tables)
            {
                // Create an repository entity for each table
                var entity = template.ToRepository(table);

                // Set the file extensions for the template
                template.FileNameWithoutExtension = entity.Name;

                // Generate the contents based on the entity using the PathToTemplate
                template.Contents = _templateService.Generate(entity, template.PathToTemplate);

                // Write the template to disk
                _fileService.Write(template);
            }
        }

        public void GenerateRepositories(string path, bool overwrite = false)
        {
            var tables = _provider.GetTables();

            foreach (var table in tables)
            {
                var entity = new Repository()
                {
                    Namespace = "Contec.Data.Dapper.Repositories",
                    Name = $"{table.Name}Repository",
                    TableName = table.TableName,
                    ModelName = table.Name,
                    BaseRepository = "DapperRepository",
                    ImplementsInterface = $"I{table.Name}Repository",
                    ConnectionInterface = "ICrsConnection"
                };
                entity.UsingDirectives.Add("Contec.Data.Models");
                entity.UsingDirectives.Add("Contec.Data.Repositories");
                entity.UsingDirectives.Add("Contec.Data.Dapper.Connections");

                var template = new Template()
                {
                    FileNameWithoutExtension = entity.Name,
                    Contents = _templateService.Generate(entity, Models.Templates.Repository),
                    Path = path,
                    Overwrite = overwrite
                };

                _fileService.Write(template);
            }
        }

        private readonly IDatabaseProvider _provider;
        private readonly ITemplateService _templateService;
        private readonly IFileService _fileService;
    }
}