using System.Collections.Generic;

using CodeGenerator.Models;

namespace CodeGenerator.Services.Interfaces
{
    public interface IFileService
    {
        void Write(Template templates);

        void Write(List<Template> templates);
    }
}