using System.Collections.Generic;

using CodeGenerator.Formatters;

namespace CodeGenerator.Models
{
    public class Repository: Entity
    {
        public string Namespace { get; set; }

        public string TableName { get; set; }

        public string ModelName { get; set; }

        public string BaseRepository { get; set; }

        public string ImplementsInterface { get; set; }

        public string ConnectionInterface { get; set; }

        public List<string> UsingDirectives { get; set; }

        public RepositoryFormatter  Formatter { get; set; }

        public Repository()
        {
            UsingDirectives = new List<string>();
            Formatter = new RepositoryFormatter();
        }
    }
}