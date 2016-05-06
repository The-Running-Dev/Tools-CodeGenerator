namespace CodeGenerator.SqlDialects
{
    public interface ISqlDialect
    {
        string GetTablesSql { get; }

        string GetSpecificTablesSql { get;}

        string GetTableColumnsSql { get; }
    }
}