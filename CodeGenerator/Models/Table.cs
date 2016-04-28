using System.Collections.Generic;

namespace CodeGenerator.Models
{
    public class Table : Entity
    {
        public string Namespace { get; set; }

        public string TableName { get; set; }

        public List<string> UsingDirectives { get; set; }

        public List<Columns> Columns { get; set; }

        public Table()
        {
            UsingDirectives = new List<string>();
            Columns = new List<Columns>();
        }
    }
}