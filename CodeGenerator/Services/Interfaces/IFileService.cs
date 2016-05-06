using System.Collections.Generic;

using CodeGenerator.Models;

namespace CodeGenerator.Services.Interfaces
{
    public interface IFileService
    {
        List<string> Write(List<Template> templates);

        string Write(Template templates);
    }
}