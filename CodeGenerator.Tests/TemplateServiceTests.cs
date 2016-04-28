using System;

using NUnit.Framework;

using CodeGenerator.Models;
using CodeGenerator.Extensions;
using CodeGenerator.Bootstraper;
using CodeGenerator.Services.Interfaces;

namespace CodeGenerator.Tests
{
    [TestFixture]
    public class TemplateServiceTests
    {
        [Test]
        public void Should_Generate_Content_For_Table()
        {
            var model = new Table()
            {
                TableName = "Customers",
                Name = "Customers".ConvertTableName(),
                Namespace = "Contec.Data.Models"
            };
            model.UsingDirectives.Add("System");
            model.Columns.Add(new Columns() { Name = "ID".ToProperCaseWord(), DataType = "string" });
            model.Columns.Add(new Columns() { Name = "PropertyID".ToProperCaseWord(), DataType = "int" });
            model.Columns.Add(new Columns() { Name = "Property3".ToProperCaseWord(), DataType = "DateTime" });

            var modelAsString = _service.Generate(model, Models.Templates.Table);

            Console.WriteLine(modelAsString);
            Assert.IsTrue(modelAsString.IsNotEmpty());
        }

        [Test]
        public void Should_Generate_Content_For_Table_Without_UsingDirectives()
        {
            var model = new Table()
            {
                TableName = "Customers",
                Name = "Customers".ConvertTableName(),
                Namespace = "Contec.Data.Models"
            };
            model.Columns.Add(new Columns() { Name = "ID".ToProperCaseWord(), DataType = "string" });
            model.Columns.Add(new Columns() { Name = "PropertyID".ToProperCaseWord(), DataType = "int" });
            model.Columns.Add(new Columns() { Name = "Property3".ToProperCaseWord(), DataType = "DateTime" });

            var modelAsString = _service.Generate(model, Models.Templates.Table);

            Console.WriteLine(modelAsString);
            Assert.IsTrue(modelAsString.IsNotEmpty());
            Assert.IsFalse(modelAsString.Contains("using"));
        }

        [Test]
        public void Should_Generate_Content_For_IRepository()
        {
            var model = new Repository()
            {
                Namespace = "Contec.Data.Repositories",
                Name = "ICustomerRepository",
                TableName = "Customers",
                ModelName = "Customer",
                ImplementsInterface = "IRepository"
            };
            model.UsingDirectives.Add("Contec.Data.Models");

            var modelAsString = _service.Generate(model, Models.Templates.IRepository);

            Console.WriteLine(modelAsString);
            Assert.IsTrue(modelAsString.IsNotEmpty());
        }

        [Test]
        public void Should_Generate_Content_For_Repository()
        {
            var model = new Repository()
            {
                Namespace = "Contec.Data.Dapper.Repositories",
                Name = "CustomerRepository",
                TableName = "Customers",
                ModelName  = "Customer",
                BaseRepository = "DapperRepository",
                ImplementsInterface = "ICustomerRepository",
                ConnectionInterface = "ICrsConnection"
            };
            model.UsingDirectives.Add("Contec.Data.Models");
            model.UsingDirectives.Add("Contec.Data.Repositories");
            model.UsingDirectives.Add("Contec.Data.Dapper.Connections");

            var modelAsString = _service.Generate(model, Models.Templates.Repository);

            Console.WriteLine(modelAsString);
            Assert.IsTrue(modelAsString.IsNotEmpty());
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Bootstrapper.Bootstrap(BootstrapType.IntegrationTest);

            _service = IocWrapper.Instance.GetService<ITemplateService>();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

        }

        private ITemplateService _service;
    }
}