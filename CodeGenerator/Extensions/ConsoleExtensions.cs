using System;

namespace CodeGenerator.Extensions
{
    public static class ConsoleExtensions
    {
        public static void ToConsole(this string contents)
        {
            Console.WriteLine(contents);
        }
    }
}