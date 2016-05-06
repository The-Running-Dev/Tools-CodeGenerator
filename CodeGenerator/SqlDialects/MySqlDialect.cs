namespace CodeGenerator.SqlDialects
{
    public class MySqlDialect : ISqlDialect
    {
        public string GetTablesSql { get; } = @"
select  table_name
from    information_schema.tables
where   table_schema = database()";

        public string GetSpecificTablesSql { get; } = @"
select  table_name
from    information_schema.tables
where   table_name in ({0})
and     table_schema = database()";

        public string GetTableColumnsSql { get; } = @"
select  column_name, data_type
from    information_schema.columns
where   table_name = '{0}'";
    }
}