using System.IO;

namespace CodeGenerator.Extensions
{
    public static class FileExtensions
    {
        public static bool Exists(this string path)
        {
            var isFile = File.Exists(path);
            var isDirectory = Directory.Exists(path);

            return path.IsNotEmpty() && (isFile || isDirectory);
        }

        public static void Delete(this string path, bool unless = false)
        {
            if (unless) return;

            if (path.IsNotEmpty())
            {
                if (File.Exists(path)) File.Delete(path);
                if (Directory.Exists(path)) Directory.Delete(path, true);
            }
        }

        public static string ReadAllText(this string path)
        {
            return File.ReadAllText(path);
        }

        public static void WriteAllText(this string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public static string DirectoryPath(this string path)
        {
            return Path.GetDirectoryName(path);
        }

        public static string DirectoryName(this string path)
        {
            return Path.GetFileName(Path.GetDirectoryName(path));
        }
    }
}