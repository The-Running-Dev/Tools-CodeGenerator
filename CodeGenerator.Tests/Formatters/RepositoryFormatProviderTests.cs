using NUnit.Framework;

using CodeGenerator.Formatters;

namespace CodeGenerator.Tests.Formatters
{
    [TestFixture]
    public class DatabaseFormatProviderTests
    {
        [Test]
        public void Should_Not_Change_Name()
        {
            var tableName = "Customer";
            var expected = "Customer";

            var actual = string.Format(new DatabaseFormatProvider(), "{0}", tableName);

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Not_Change_Name_For_Invalid_Format_Specified()
        {
            var tableName = "Inventories";
            var expected = "Inventory";

            var actual = string.Format(new DatabaseFormatProvider(), "{0:S}", tableName);

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Lower_Case_Name()
        {
            var tableName = "Customer";
            var expected = "customer";

            var actual = string.Format(new DatabaseFormatProvider(), "{0:L}", tableName);

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Upper_Case_Name()
        {
            var tableName = "Customer";
            var expected = "CUSTOMER";

            var actual = string.Format(new DatabaseFormatProvider(), "{0:U}", tableName);

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Pluralize_Name()
        {
            var tableName = "Customer";
            var expected = "Customers";

            var actual = string.Format(new DatabaseFormatProvider(), "{0:P}", tableName);

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Singular_Name()
        {
            var tableName = "Inventories";
            var expected = "Inventory";

            var actual = string.Format(new DatabaseFormatProvider(), "{0:S}", tableName);

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}