using System.Linq;
using System.Collections.Generic;

using MySql.Data.MySqlClient;

using CodeGenerator.Models;
using CodeGenerator.Extensions;
using CodeGenerator.SqlDialects;
using CodeGenerator.Configuration;

namespace CodeGenerator.Providers
{
    public class MySqlProvider : IDatabaseProvider
    {
        public MySqlProvider(IConfiguration configuration, ISqlDialect sqlDialect)
        {
            _configuration = configuration;
            _sqlDialect = sqlDialect;

            _connectionString = _configuration.GetConnectionString("DatabaseConnection");
        }

        public List<Table> GetTables(List<string> tableNames = null)
        {
            var tables = new List<Table>();
            var commandSql = _sqlDialect.GetTablesSql;

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                if (tableNames != null && tableNames.Any())
                {
                    commandSql = string.Format(_sqlDialect.GetSpecificTablesSql, tableNames.AsString("'{0}'"));
                }

                // Create the command to get the tables
                var command = new MySqlCommand(commandSql, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tableName = reader.GetValue(0).ToString();

                        var model = new Table
                        {
                            TableName = ((tableNames != null && tableNames.Any())) ? tableNames.FirstOrDefault(item => item.IsEqualTo(tableName)) : tableName,
                            Columns = GetTableColumns(tableName)
                        };

                        // Add each table to the list of tables
                        tables.Add(model);
                    }
                }

                connection.Close();
            }

            return tables;
        }

        public List<Columns> GetTableColumns(string tableName)
        {
            var columns = new List<Columns>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var command = new MySqlCommand(string.Format(_sqlDialect.GetTableColumnsSql, tableName), connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var columnName = reader.GetValue(0).ToString().ToProperCaseWord();
                        var columnDataType = reader.GetValue(1).ToString();
                        var mappedDataType = columnDataType.ToDataType();

                        columns.Add(new Columns
                        {
                            Name = columnName,
                            DataType = !string.IsNullOrWhiteSpace(mappedDataType) ? mappedDataType : columnDataType
                        });
                    }
                }

                connection.Close();
            }

            return columns;
        }

        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly ISqlDialect _sqlDialect;
    }
}