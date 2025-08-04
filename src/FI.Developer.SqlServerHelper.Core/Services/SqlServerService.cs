using FI.Developer.SqlServerHelper.Core.Models;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FI.Developer.SqlServerHelper.Core.Services
{
    public class SqlServerService
    {
        private readonly string _connectionString;

        public SqlServerService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<TableInfo>> GetTablesAsync(string schemaName = null)
        {
            var tables = new List<TableInfo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var serverConnection = new ServerConnection(connection);
                var server = new Server(serverConnection);
                var database = server.Databases[connection.Database];

                foreach (Table table in database.Tables)
                {
                    if (!table.IsSystemObject && (string.IsNullOrEmpty(schemaName) || table.Schema == schemaName))
                    {
                        var tableInfo = new TableInfo
                        {
                            SchemaName = table.Schema,
                            TableName = table.Name,
                            DatabaseName = database.Name
                        };

                        foreach (Column column in table.Columns)
                        {
                            var columnInfo = new ColumnInfo
                            {
                                Name = column.Name,
                                DataType = column.DataType.Name,
                                MaxLength = column.DataType.MaximumLength,
                                Precision = column.DataType.NumericPrecision,
                                Scale = column.DataType.NumericScale,
                                IsNullable = column.Nullable,
                                IsIdentity = column.Identity,
                                DefaultValue = column.Default
                            };

                            tableInfo.Columns.Add(columnInfo);
                        }

                        foreach (Microsoft.SqlServer.Management.Smo.Index index in table.Indexes)
                        {
                            if (index.IndexKeyType == IndexKeyType.DriPrimaryKey)
                            {
                                foreach (IndexedColumn indexCol in index.IndexedColumns)
                                {
                                    var col = tableInfo.Columns.FirstOrDefault(c => c.Name == indexCol.Name);
                                    if (col != null)
                                    {
                                        col.IsPrimaryKey = true;
                                        tableInfo.PrimaryKeyColumns.Add(col);

                                    }
                                }
                            }
                        }

                        tables.Add(tableInfo);
                    }
                }

                return tables;
            }
        }

        public async Task<TableInfo> GetTableInfoAsync(string schemaName, string tableName)
        {
            var tables = await GetTablesAsync(schemaName);
            return tables.FirstOrDefault(t => t.TableName == tableName);
        }
    }
}
