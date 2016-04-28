using System.Collections.Generic;

using MySql.Data.MySqlClient;

using CodeGenerator.Models;
using CodeGenerator.Extensions;

namespace CodeGenerator.Providers
{
    public class MySqlProvider: IDatabaseProvider
    {
        // ToDo: Move database name extraction from connection string
        public string DatabaseName = "crs_dev";

        // ToDo: Pass the connection string through DI with IConfiguration
        public string ConnectionString = "";

        public MySqlProvider()
        {
            
        }

        public List<Table> GetTables()
        {
            var tables = new List<Table>();

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                // Create the command to get the tables
                var command = new MySqlCommand(string.Format(getTablesSql, DatabaseName), connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tableName = reader.GetValue(0).ToString();

                        var model = new Table
                        {
                            TableName = tableName,
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

            using (var connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                var command = new MySqlCommand(string.Format(getTableColumnsSql, tableName), connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var columnName = reader.GetValue(0).ToString().ToProperCaseWord();
                        var columnDataType = reader.GetValue(1).ToString();
                        var mappedDataType = columnDataType.ToDataType();

                        // If we can't map it, skip it
                        //if (string.IsNullOrWhiteSpace(columnDataType))
                        //{
                        //    // Skip
                        //    continue;
                        //}

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

        private string getTablesSql = @"
select  table_name
from    information_schema.tables
where   table_schema = '{0}'";

        private string getTableColumnsSql = @"
select  column_name, data_type
from    information_schema.columns
where   table_name = '{0}'";
    }
}