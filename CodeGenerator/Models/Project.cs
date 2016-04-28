using System.Collections.Generic;

namespace CodeGenerator.Models
{
    public class Project
    {
        public string Path { get; set; } 

        public List<string> Includes { get; set; }

        public Project()
        {
            Includes = new List<string>();
        }
    }
}