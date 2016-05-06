using System.Collections.Generic;

using NUnit.Framework;

using CodeGenerator.Extensions;

namespace CodeGenerator.Tests
{
    [TestFixture]
    public class ListExtensionsTests
    {
        [Test]
        public void Should_Convert_Single_Item_List_To_String()
        {
            var stringList = new List<string> { "String 1" };
            var expected = "String 1";

            var actual = stringList.AsString();

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Convert_Single_Item_List_To_String_With_Format()
        {
            var stringList = new List<string> { "String 1" };
            var expected = "==String 1==";

            var actual = stringList.AsString("=={0}==");

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Convert_Multiple_Item_List_To_String()
        {
            var stringList = new List<string> { "String 1", "String 2", "String 3" };
            var expected = "String 1,String 2,String 3";

            var actual = stringList.AsString();

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Convert_Multiple_Item_List_To_String_With_Delimiter()
        {
            var stringList = new List<string> { "String 1", "String 2", "String 3" };
            var expected = "String 1 - String 2 - String 3";

            var actual = stringList.AsString(delimiter: " - ");

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Convert_Multiple_Item_List_To_String_With_Format()
        {
            var stringList = new List<string> { "String 1", "String 2", "String 3" };
            var expected = "'String 1','String 2','String 3'";

            var actual = stringList.AsString("'{0}'");

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Should_Convert_Multiple_Item_List_To_String_With_Format_And_Delimiter()
        {
            var stringList = new List<string> { "String 1", "String 2", "String 3" };
            var expected = "'String 1' == 'String 2' == 'String 3'";

            var actual = stringList.AsString("'{0}'", " == ");

            Assert.IsNotEmpty(actual);
            Assert.AreEqual(expected, actual);
        }

    }
}