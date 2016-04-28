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
    public class MySqlGeneratorTests
    {
        [Test]
        public void Should_Generate_Repositories()
        {
            var path = Path.Combine(SampleData.GeneratedDirectoryPath, "Repositories");

            // Generate the models
            _service.GenerateRepositories(path);

            Assert.IsTrue(path.Exists());

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
        private static bool DoCleanup = true;
    }
}