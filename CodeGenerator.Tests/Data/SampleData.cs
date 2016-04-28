using System;
using System.IO;
using System.Reflection;

using CodeGenerator.Models;
using CodeGenerator.Extensions;

namespace CodeGenerator.Tests.Data
{
    public static class SampleData
    {
        public static string AssemlbyDirectoryPath => Assembly.GetExecutingAssembly().Location.DirectoryPath();

        public static string GeneratedDirectoryPath => Path.Combine(AssemlbyDirectoryPath, "Generated");

        public static Template CreateTemplate(string name, string randomText)
        {
            var sampleContents =$"public class SomeClass_{randomText} {{ }}";

            return new Template()
            {
                FileNameWithoutExtension = name,
                Path = AssemlbyDirectoryPath,
                Contents = sampleContents
            };
        }

        public static string RandomText()
        {
            return new Random().Next(1, 100000).ToString();
        }
    }
}