using System.IO;
using System.Linq;

using NUnit.Framework;

using CodeGenerator.Models;
using CodeGenerator.Extensions;
using CodeGenerator.Bootstraper;

using CodeGenerator.Tests.Data;

namespace CodeGenerator.Tests
{
    [TestFixture]
    public class CodeGeneratorTests
    {
        [Test]
        public void Should_Generate_Models()
        {
            _defaultModelPath.Delete(!_preRunCleanup);

            var template = new Template()
            {
                Namespace = _defaultModelNamespace,
                Path = _defaultModelPath,
                PathToTemplate = Templates.Table,
                Overwrite = true,
                Entities = Entities.ToList()
            };
            template.UsingDirectives.Add("System");

            var generatedFiles = _service.GenerateModels(template);

            foreach (var file in generatedFiles)
            {
                file.FileName().ToConsole();

                Assert.IsNotEmpty(file.ReadAllText());
            }

            _defaultModelPath.Delete(!_postRunCleanup);
        }

        [Test]
        public void Should_Generate_Repository_Interfaces()
        {
            _defaultIRepositoriesPath.Delete(!_preRunCleanup);

            var template = new Template()
            {
                Namespace = _defaultIRepositoryNamespace,
                Path = _defaultIRepositoriesPath,
                PathToTemplate = Templates.IRepository,
                Overwrite = true,
                Entity = new Repository()
                {
                    Name = "I{TableName:S}Repository",
                    TableName = "{TableName}",
                    ModelName = "{TableName:S}",
                    ImplementsInterface = "IRepository"
                },
                Entities = Entities.ToList()
            };
            template.UsingDirectives.Add("Contec.Data.Models");

            _service.GenerateRepositoryInterfaces(template);

            foreach (var file in Directory.GetFiles(_defaultIRepositoriesPath))
            {
                file.FileName().ToConsole();

                Assert.IsNotEmpty(file.ReadAllText());
            }

            _defaultIRepositoriesPath.Delete(!_postRunCleanup);
        }

        [Test]
        public void Should_Generate_Repositories()
        {
            _defaultRepositoriesPath.Delete(!_preRunCleanup);

            var template = new Template()
            {
                Namespace = _defaultRepositoryNamespace,
                Path = _defaultRepositoriesPath,
                PathToTemplate = Templates.Repository,
                Overwrite = true,
                Entity = new Repository()
                {
                    Name = "{TableName:S}Repository",
                    TableName = "{TableName}",
                    ModelName = "{TableName:S}",
                    BaseRepository = "DapperRepository",
                    ImplementsInterface = "I{TableName:S}Repository",
                    ConnectionInterface = "ICrsConnection"
                },
                Entities = Entities.ToList()
            };
            template.UsingDirectives.Add("Contec.Data.Models");
            template.UsingDirectives.Add("Contec.Data.Repositories");
            template.UsingDirectives.Add("Contec.Data.Dapper.Connections");

            _service.GenerateRepositories(template);

            foreach (var file in Directory.GetFiles(_defaultRepositoriesPath))
            {
                file.FileName().ToConsole();

                Assert.IsNotEmpty(file.ReadAllText());
            }

            _defaultRepositoriesPath.Delete(!_postRunCleanup);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Bootstrapper.Bootstrap(BootstrapType.IntegrationTest);

            _service = IocWrapper.Instance.GetService<MySqlGenerator>();

            _preRunCleanup = false;
            _postRunCleanup = false;

            _defaultModelPath = @"D:\Dropbox\Contec\Projects\ContecIT\Contec.MVC\Contec.Data\Models";
            _defaultIRepositoriesPath = @"D:\Dropbox\Contec\Projects\ContecIT\Contec.MVC\Contec.Data\Repositories";
            _defaultRepositoriesPath = @"D:\Dropbox\Contec\Projects\ContecIT\Contec.MVC\Contec.Data.Dapper\Repositories";
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }

        private MySqlGenerator _service;

        private static readonly string[] Entities = { "InventoryPieces" };

        private static string _defaultModelNamespace = "Contec.Data.Models";
        private static string _defaultIRepositoryNamespace = "Contec.Data.Repositories";
        private static string _defaultRepositoryNamespace = "Contec.Data.Dapper.Repositories";

        private static bool _postRunCleanup = false;
        private static bool _preRunCleanup = false;

        private static string _defaultModelPath = Path.Combine(SampleData.GeneratedDirectoryPath, "Models");
        private static string _defaultIRepositoriesPath = Path.Combine(SampleData.GeneratedDirectoryPath, "IRepositories");
        private static string _defaultRepositoriesPath = Path.Combine(SampleData.GeneratedDirectoryPath, "Repositories");
    }
}