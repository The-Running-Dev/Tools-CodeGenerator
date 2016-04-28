using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

using CodeGenerator.Models;
using CodeGenerator.Extensions;
using CodeGenerator.Bootstraper;
using CodeGenerator.Services.Interfaces;

using CodeGenerator.Tests.Data;

namespace CodeGenerator.Tests
{
    [TestFixture]
    public class ProjectServiceTests
    {
        [Test]
        public void Should_Build_Include_Path()
        {
            var fileName = "SomeClass.cs";

            var includePath = _service.BuildIncludePath(_sampleProjectFilePath, fileName);

            Assert.IsNotEmpty(includePath);
            Assert.AreEqual($"{_sampleProjectFilePath.DirectoryName()}\\{fileName}", includePath);
        }

        [Test]
        public void Should_Add_Compile_Include_To_Project_File()
        {
            var template = SampleData.CreateTemplate("SomeClass", SampleData.RandomText());

            _service.Load(_sampleProjectFilePath);
            _service.AddCompileInclude(_sampleProjectFilePath, template);
            _service.Save();

            foreach (var projectItem in _service.Project.Items.Where(item => item.ItemType == ProjectItemType.Compile))
            {
                projectItem.EvaluatedInclude.ToConsole();
            }

            var projectFileAsString = File.ReadAllText(_sampleProjectFilePath);

            Assert.IsTrue(_service.Project.Items.Any(item => item.EvaluatedInclude.Contains(template.FileName)));
            Assert.IsTrue(projectFileAsString.Contains(template.FileName));
        }

        [Test]
        public void Should_Add_Multiple_Compile_Includes_To_Project_File()
        {
            var templates = new List<Template>
            {
                SampleData.CreateTemplate("SomeClass1", SampleData.RandomText()),
                SampleData.CreateTemplate("SomeClass2", SampleData.RandomText()),
                SampleData.CreateTemplate("SomeClass3", SampleData.RandomText())
            };

            _service.Load(_sampleProjectFilePath);
            _service.AddCompileIncludes(_sampleProjectFilePath, templates);
            _service.Save();

            foreach (var projectItem in _service.Project.Items.Where(item => item.ItemType == ProjectItemType.Compile))
            {
                projectItem.EvaluatedInclude.ToConsole();
            }

            var projectFileAsString = File.ReadAllText(_sampleProjectFilePath);

            foreach (var template in templates)
            {
                Assert.IsTrue(_service.Project.Items.Any(item => item.EvaluatedInclude.Contains(template.FileName)));
                Assert.IsTrue(projectFileAsString.Contains(template.FileName));
            }
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Bootstrapper.Bootstrap(BootstrapType.IntegrationTest);

            _service = IocWrapper.Instance.GetService<IProjectService>();

            _sampleProjectFilePath = Path.Combine(SampleData.AssemlbyDirectoryPath, $"Data\\{SampleProjectFile}");
            _sampleEmptyProjectFilePath = Path.Combine(SampleData.AssemlbyDirectoryPath, $"Data\\{SampleEmptyProjectFile}");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

        }

        private IProjectService _service;

        private const string SampleProjectFile = "Sample.Data.csproj";
        private const string SampleEmptyProjectFile = "Sample.Data.Empty.csproj";

        private string _sampleProjectFilePath;
        private string _sampleEmptyProjectFilePath;
    }
}