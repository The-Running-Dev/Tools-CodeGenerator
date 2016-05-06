using CodeGenerator.Models;
using CodeGenerator.Extensions;

using NUnit.Framework;

namespace CodeGenerator.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void Should_Replace_Single_Token()
        {
            var expected = "ICustomersRepository";

            var t = new Repository
            {
                Name = "I{TableName}Repository",
                TableName = "Customers",
            };

            var newString = t.Name.TokensToValue(t);

            Assert.IsNotEmpty(newString);
            Assert.AreEqual(expected, newString);
        }

        [Test]
        public void Should_Replace_Single_Token_With_Formatter()
        {
            var expected = "ICustomerRepository";

            var t = new Repository
            {
                Name = "I{TableName}Repository",
                TableName = "Customers",
            };

            var newString = t.Name.TokensToValue(t, (token) => token.PluralToSingular());

            Assert.IsNotEmpty(newString);
            Assert.AreEqual(expected, newString);
        }

        [Test]
        public void Should_Replace_Multiple_Token()
        {
            var expected = "ICustomerInventoryRepository";

            var t = new Repository
            {
                Name = "I{TableName}{ModelName}Repository",
                TableName = "Customer",
                ModelName = "Inventory"
            };

            var actual = t.Name.TokensToValue(t);

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Replace_Multiple_Token_With_Formatter()
        {
            var expected = "ICustomerInventoryRepository";

            var t = new Repository
            {
                Name = "I{TableName}{ModelName}Repository",
                TableName = "Customers",
                ModelName = "Inventories"
            };

            var actual = t.Name.TokensToValue(t, (token) => token.PluralToSingular());

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Replace_All_Tokens()
        {
            var expected = "CustomerInventory";

            var t = new Repository
            {
                Name = "{TableName}{ModelName}",
                TableName = "Customer",
                ModelName = "Inventory"
            };

            var newString = t.Name.TokensToValue(t);

            Assert.IsNotEmpty(newString);
            Assert.AreEqual(expected, newString);
        }

        [Test]
        public void Should_Use_Customer_Formatter_To_Convert_To_Singular()
        {
            var expected = "CustomerModel";

            var t = new Repository
            {
                TableName = "Customers",
            };

            var actual = "{TableName}Model".TokensToValue(t, (token) => token.PluralToSingular());

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Use_Customer_Formatter_To_Convert_To_Lower()
        {
            var expected = "inventoryModel";

            var t = new Repository
            {
                TableName = "{ModelName}Model",
                ModelName = "Inventory"
            };

            var actual = "{ModelName}Model".TokensToValue(t, (token) => token.ToLower());

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}