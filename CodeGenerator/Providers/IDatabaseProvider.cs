using System.Collections.Generic;

using CodeGenerator.Models;

namespace CodeGenerator.Providers
{
    public interface IDatabaseProvider
    {
        List<Table> GetTables();

        List<Columns> GetTableColumns(string tableName);
    }
}