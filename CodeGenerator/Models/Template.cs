using System.Collections.Generic;

namespace CodeGenerator.Models
{
    public class Template
    {
        public string FileNameWithoutExtension { get; set; }

        public string Namespace { get; set; }

        public string FileName => string.Format(FileNameFormat, FileNameWithoutExtension);

        public string FileNameFormat { get; set; }

        public string Path { get; set; }

        public bool Overwrite { get; set; }

        public IEntity Entity { get; set; }

        public string PathToTemplate { get; set; }

        public List<string> UsingDirectives { get; set; }

        public string Contents { get; set; }

        public Template()
        {
            FileNameFormat = CsFileNameFormat;
            UsingDirectives = new List<string>();
        }

        private const string CsFileNameFormat = "{0}.cs";
    }
}