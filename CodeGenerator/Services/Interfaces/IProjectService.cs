using System.Collections.Generic;

using Microsoft.Build.Evaluation;

using CodeGenerator.Models;

namespace CodeGenerator.Services.Interfaces
{
    public interface IProjectService
    {
        Microsoft.Build.Evaluation.Project Project { get; set; }

        void Load(string path);

        void Save();

        string BuildIncludePath(string path, string fileName);

        void AddCompileIncludes(string path, List<Template> templates);

        void AddCompileInclude(string path, Template template);

        bool IncludeExists(ICollection<ProjectItem> projectItems, string itemType, string itemPath);
    }
}