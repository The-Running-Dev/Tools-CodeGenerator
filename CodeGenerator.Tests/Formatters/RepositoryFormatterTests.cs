using NUnit.Framework;

using CodeGenerator.Models;
using CodeGenerator.Formatters;

namespace CodeGenerator.Tests.Formatters
{
    [TestFixture]
    public class RepositoryFormatterTests
    {
        [Test]
        public void Should_Lower_Case_Name()
        {
            var expected = "customer";

            var t = new Repository
            {
                TableName = "Customer",
            };

            var actual = new RepositoryFormatter().Format("{TableName:L}", t);

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Upper_Case_Name()
        {
            var expected = "CUSTOMER";

            var t = new Repository
            {
                TableName = "Customer",
            };

            var actual = new RepositoryFormatter().Format("{TableName:U}", t);

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Pluralize_Name()
        {
            var expected = "Customers";

            var t = new Repository
            {
                TableName = "Customer",
            };

            var actual = new RepositoryFormatter().Format("{TableName:P}", t);

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Singular_Name()
        {
            var expected = "Inventory";

            var t = new Repository
            {
                TableName = "Inventories"
            };

            var actual = new RepositoryFormatter().Format("{TableName:S}", t);

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}