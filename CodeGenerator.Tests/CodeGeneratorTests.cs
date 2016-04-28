using System.IO;
using System.Collections.Generic;

using NUnit.Framework;

using CodeGenerator.Models;
using CodeGenerator.Extensions;
using CodeGenerator.Bootstraper;
using CodeGenerator.Services.Interfaces;

using CodeGenerator.Tests.Data;

namespace CodeGenerator.Tests
{
    [TestFixture]
    public class CodeGeneratorTests
    {
        [Test]
        public void Should_Generate_Models()
        {
            var path = Path.Combine(SampleData.GeneratedDirectoryPath, "Models");

            var template = new Template()
            {
                Namespace = "Contec.Data.Models",
                Path = path,
                PathToTemplate = Models.Templates.Table,
                Overwrite = true
            };
            template.UsingDirectives.Add("System");

            _service.GenerateModels(template);

            foreach (var file in Directory.GetFiles(path))
            {
                file.ToConsole();

                Assert.IsNotEmpty(file.ReadAllText());
            }

            path.Delete(!DoCleanup);
        }

        [Test]
        public void Should_Generate_Repository_Interfaces()
        {
            var path = Path.Combine(SampleData.GeneratedDirectoryPath, "IRepositories");

            var template = new Template()
            {
                Namespace = "Contec.Data.Repositories",
                Path = path,
                PathToTemplate = Models.Templates.IRepository,
                Overwrite = true,
                Entity = new Repository()
                {
                    Name = $"I{{TableName}}Repository",
                    ImplementsInterface = "IRepository"
                }
            };
            template.UsingDirectives.Add("Contec.Data.Models");

            _service.GenerateRepositoryInterfaces(template);

            foreach (var file in Directory.GetFiles(path))
            {
                file.ToConsole();

                Assert.IsNotEmpty(file.ReadAllText());
            }

            path.Delete(!DoCleanup);
        }

        [Test]
        public void Should_Generate_Repositories()
        {
            var path = Path.Combine(SampleData.GeneratedDirectoryPath, "Repositories");

            var template = new Template()
            {
                Namespace = "Contec.Data.Dapper.Repositories",
                Path = path,
                PathToTemplate = Models.Templates.Repository,
                Overwrite = true,
                Entity = new Repository()
                {
                    Name = $"{{TableName}}Repository",
                    BaseRepository = "DapperRepository",
                    ImplementsInterface = $"I{{TableName}}Repository",
                    ConnectionInterface = "ICrsConnection"
                }
            };
            template.UsingDirectives.Add("Contec.Data.Models");
            template.UsingDirectives.Add("Contec.Data.Repositories");
            template.UsingDirectives.Add("Contec.Data.Dapper.Connections");

            _service.GenerateRepositories(template);

            foreach (var file in Directory.GetFiles(path))
            {
                file.ToConsole();

                Assert.IsNotEmpty(file.ReadAllText());
            }

            path.Delete(!DoCleanup);
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Bootstrapper.Bootstrap(BootstrapType.IntegrationTest);

            _service = IocWrapper.Instance.GetService<MySqlGenerator>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

        }

        private MySqlGenerator _service;
        private static bool DoCleanup = false;
    }
}