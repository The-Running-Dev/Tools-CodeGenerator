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
    public class FileServiceTests
    {
        [Test]
        public void Should_Write_Single_File()
        {
            var randomText = SampleData.RandomText();
            var template = SampleData.CreateTemplate("SomeClass", randomText);
            var outputFile = Path.Combine(SampleData.AssemlbyDirectoryPath, template.FileName);

            // Delete the file if it already exists
            if (outputFile.Exists()) File.Delete(outputFile);

            // Write the template
            _service.Write(template);

            var fileExists = outputFile.Exists();
            var fileContents = string.Empty;

            if (fileExists) fileContents = outputFile.ReadAllText();

            Assert.IsTrue(fileExists);
            Assert.IsNotEmpty(fileContents);
            Assert.IsTrue(fileContents.Contains(randomText));

            outputFile.Delete();
        }

        [Test]
        public void Should_Not_Write_File_If_File_Exists()
        {
            var randomText = SampleData.RandomText();
            var template = SampleData.CreateTemplate("SomeClass", randomText);

            // Write some random text to a file with the same name
            var outputFile = Path.Combine(SampleData.AssemlbyDirectoryPath, template.FileName);
            outputFile.WriteAllText("Some Random Text");

            // Try to write the template
            _service.Write(template);

            var fileContents = outputFile.ReadAllText();

            Assert.IsTrue(outputFile.Exists());
            Assert.IsNotEmpty(fileContents);
            Assert.IsFalse(fileContents.Contains(randomText));

            outputFile.Delete();
        }

        [Test]
        public void Should_Write_Multiple_Files()
        {
            string outputFile;

            var templates = new List<Template>
            {
                SampleData.CreateTemplate("SomeClass1", SampleData.RandomText()),
                SampleData.CreateTemplate("SomeClass2", SampleData.RandomText()),
                SampleData.CreateTemplate("SomeClass3", SampleData.RandomText())
            };

            foreach (var template in templates)
            {
                outputFile = Path.Combine(SampleData.AssemlbyDirectoryPath, template.FileName);

                // Delete the file if it already exists
                if (outputFile.Exists()) File.Delete(outputFile);
            }

            // Write all the template
            _service.Write(templates);

            foreach (var template in templates)
            {
                outputFile = Path.Combine(SampleData.AssemlbyDirectoryPath, template.FileName);

                var fileExists = outputFile.Exists();
                var fileContents = string.Empty;
                if (fileExists) fileContents = outputFile.ReadAllText();

                Assert.IsTrue(fileExists);
                Assert.IsNotEmpty(fileContents);

                outputFile.Delete();
            }
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Bootstrapper.Bootstrap(BootstrapType.IntegrationTest);

            _service = IocWrapper.Instance.GetService<IFileService>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

        }

        private IFileService _service;
    }
}