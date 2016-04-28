using System.Linq;
using System.Collections.Generic;

using Microsoft.Build.Evaluation;

using CodeGenerator.Models;
using CodeGenerator.Extensions;
using CodeGenerator.Services.Interfaces;

namespace CodeGenerator.Services
{
    public class ProjectService : IProjectService
    {
        public Microsoft.Build.Evaluation.Project Project { get; set; }

        public void Load(string path)
        {
            Project = new Microsoft.Build.Evaluation.Project(path);
        }

        public void Save()
        {
            Project.Save();

            // Unload the project from the global cache
            ProjectCollection.GlobalProjectCollection.UnloadProject(Project);
        }

        public string BuildIncludePath(string path, string fileName)
        {
            return string.Format($"{path.DirectoryName()}\\{fileName}");
        }

        public void AddCompileIncludes(string path, List<Template> templates)
        {
            foreach (var template in templates)
            {
                AddCompileInclude(path, template);
            }
        }

        public void AddCompileInclude(string path,  Template template)
        {
            var includeFilePath = BuildIncludePath(path, template.FileName);

            if (!IncludeExists(Project.Items, "Compile", includeFilePath))
            {
                Project.AddItem("Compile", includeFilePath);
            }
        }

        public bool IncludeExists(ICollection<ProjectItem> projectItems, string itemType, string itemPath)
        {
            return projectItems.Any(
                item => item.ItemType.IsEqualTo(itemType)
                && item.EvaluatedInclude.IsEqualTo(itemPath));
        }
    }
}