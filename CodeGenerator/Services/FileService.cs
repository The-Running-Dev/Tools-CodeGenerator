using System;
using System.IO;
using System.Collections.Generic;

using CodeGenerator.Models;
using CodeGenerator.Extensions;
using CodeGenerator.Services.Interfaces;

namespace CodeGenerator.Services
{
    /// <summary>
    /// Implements the IFileService interface
    /// by writing the templates to file system
    /// </summary>
    public class FileService: IFileService
    {
        public List<string> Write(List<Template> templates)
        {
            var writtenFiles = new List<string>();

            foreach (var template in templates)
            {
                var path = Write(template);

                if (path.IsNotEmpty()) writtenFiles.Add(path);
            }

            return writtenFiles;
        }

        public string Write(Template template)
        {
            var path = Path.Combine(template.Path, template.FileName);

            try
            {
                // Create the directory
                Directory.CreateDirectory(template.Path);

                if (!template.Overwrite && path.Exists())
                {
                    $"File {path} Exists...Skipping".ToConsole();

                    return string.Empty;
                }

                path.Delete();
                path.WriteAllText(template.Contents);
            }
            catch (Exception ex)
            {
                $"File {path} Failed to Save".ToConsole();
            }

            return path;
        }
    }
}