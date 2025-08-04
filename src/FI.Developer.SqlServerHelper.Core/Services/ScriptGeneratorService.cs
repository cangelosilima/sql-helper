using FI.Developer.SqlServerHelper.Core.Interfaces;
using FI.Developer.SqlServerHelper.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FI.Developer.SqlServerHelper.Core.Services
{
    public class ScriptGeneratorService : IScriptGenerator
    {
        private readonly ITemplateManager _templateManager;

        public ScriptGeneratorService(ITemplateManager templateManager)
        {
            _templateManager = templateManager;
        }

        public string GenerateUpsertProcedure(TableInfo table)
        {
            var sb = new StringBuilder();

            // Header
            sb.AppendLine($"CREATE OR ALTER PROCEDURE [{table.SchemaName}].[Sp_{table.DatabaseName}_{table.TableName}_Ins]");

            // Parameters
            var parameters = new List<string>();
            var identityColumn = table.Columns.FirstOrDefault(c => c.IsIdentity);

            foreach (var column in table.Columns.Where(c => !c.Name.Equals("Id_Usu", StringComparison.OrdinalIgnoreCase)
                && !c.Name.Equals("Dt_Atuliz", StringComparison.OrdinalIgnoreCase)))
            {
                var param = $"    @{column.Name} {GetSqlDataType(column)}";

                if (column.IsIdentity)
                {
                    param += " OUTPUT";
                }
                else if (!column.IsNullable && string.IsNullOrEmpty(column.DefaultValue))
                {
                    // Required parameter
                }
                else
                {
                    param += " = NULL";
                }

                parameters.Add(param);
            }

            sb.AppendLine(string.Join(",\n", parameters));
            sb.AppendLine("AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine("    SET NOCOUNT ON;");
            sb.AppendLine();

            // Update attempt
            sb.AppendLine("    -- Try to update existing record");
            sb.AppendLine($"    UPDATE [{table.SchemaName}].[{table.TableName}]");
            sb.AppendLine("    SET");

            var updateColumns = table.Columns
                .Where(c => !c.IsIdentity && !c.IsPrimaryKey
                    && !c.Name.Equals("Id_Usu", StringComparison.OrdinalIgnoreCase)
                    && !c.Name.Equals("Dt_Atuliz", StringComparison.OrdinalIgnoreCase))
                .Select(c => $"        [{c.Name}] = @{c.Name}")
                .ToList();

            updateColumns.Add("        [Dt_Atuliz] = GETDATE()");

            sb.AppendLine(string.Join(",\n", updateColumns));
            sb.AppendLine("    WHERE");

            var whereConditions = table.PrimaryKeyColumns
                .Where(c => !c.IsIdentity)
                .Select(c => $"        [{c.Name}] = @{c.Name}")
                .ToList();

            if (identityColumn != null && table.PrimaryKeyColumns.Contains(identityColumn))
            {
                whereConditions.Add($"        [{identityColumn.Name}] = @{identityColumn.Name}");
            }

            sb.AppendLine(string.Join(" AND\n", whereConditions));
            sb.AppendLine();

            // Insert if update didn't affect any rows
            sb.AppendLine("    -- Insert if no rows were updated");
            sb.AppendLine("    IF @@ROWCOUNT = 0");
            sb.AppendLine("    BEGIN");
            sb.AppendLine($"        INSERT INTO [{table.SchemaName}].[{table.TableName}]");
            sb.AppendLine("        (");

            var insertColumns = table.Columns
                .Where(c => !c.IsIdentity && !c.Name.Equals("Dt_Atuliz", StringComparison.OrdinalIgnoreCase))
                .Select(c => $"            [{c.Name}]")
                .ToList();

            insertColumns.Add("            [Dt_Atuliz]");

            sb.AppendLine(string.Join(",\n", insertColumns));
            sb.AppendLine("        )");
            sb.AppendLine("        VALUES");
            sb.AppendLine("        (");

            var insertValues = table.Columns
                .Where(c => !c.IsIdentity && !c.Name.Equals("Dt_Atuliz", StringComparison.OrdinalIgnoreCase))
                .Select(c => $"            @{c.Name}")
                .ToList();

            insertValues.Add("            GETDATE()");

            sb.AppendLine(string.Join(",\n", insertValues));
            sb.AppendLine("        );");

            if (identityColumn != null)
            {
                sb.AppendLine();
                sb.AppendLine($"        SET @{identityColumn.Name} = SCOPE_IDENTITY();");
            }

            sb.AppendLine("    END");
            sb.AppendLine("END");

            return sb.ToString();
        }

        public string GenerateDeleteProcedure(TableInfo table)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"CREATE OR ALTER PROCEDURE [{table.SchemaName}].[Sp_{table.DatabaseName}_{table.TableName}_Del]");

            // Parameters - only PK columns
            var parameters = table.PrimaryKeyColumns
                .Select(c => $"    @{c.Name} {GetSqlDataType(c)}")
                .ToList();

            sb.AppendLine(string.Join(",\n", parameters));
            sb.AppendLine("AS");
            sb.AppendLine("BEGIN");
            sb.AppendLine("    SET NOCOUNT ON;");
            sb.AppendLine();
            sb.AppendLine($"    DELETE FROM [{table.SchemaName}].[{table.TableName}]");
            sb.AppendLine("    WHERE");

            var whereConditions = table.PrimaryKeyColumns
                .Select(c => $"        [{c.Name}] = @{c.Name}")
                .ToList();

            sb.AppendLine(string.Join(" AND\n", whereConditions));
            sb.AppendLine("END");

            return sb.ToString();
        }

        public string GenerateExecutionScript(string procedureName, Dictionary<string, object> parameters)
        {
            var sb = new StringBuilder();

            // Declare output parameters if any
            var outputParams = parameters.Where(p => p.Key.EndsWith("_OUTPUT")).ToList();
            foreach (var param in outputParams)
            {
                sb.AppendLine($"DECLARE @{param.Key.Replace("_OUTPUT", "")} INT;");
            }

            if (outputParams.Any())
                sb.AppendLine();

            // Execute procedure
            sb.Append($"EXEC {procedureName}");

            if (parameters.Any())
            {
                sb.AppendLine();
                var paramList = new List<string>();

                foreach (var param in parameters)
                {
                    var paramName = param.Key.Replace("_OUTPUT", "");
                    var paramValue = param.Value;

                    if (param.Key.EndsWith("_OUTPUT"))
                    {
                        paramList.Add($"    @{paramName} = @{paramName} OUTPUT");
                    }
                    else
                    {
                        string valueStr;
                        if (paramValue == null || paramValue == DBNull.Value)
                        {
                            valueStr = "NULL";
                        }
                        else if (paramValue is string || paramValue is DateTime)
                        {
                            valueStr = $"'{paramValue}'";
                        }
                        else
                        {
                            valueStr = paramValue.ToString();
                        }

                        paramList.Add($"    @{paramName} = {valueStr}");
                    }
                }

                sb.AppendLine(string.Join(",\n", paramList));
            }

            sb.AppendLine(";");

            // Select output parameters
            if (outputParams.Any())
            {
                sb.AppendLine();
                foreach (var param in outputParams)
                {
                    var paramName = param.Key.Replace("_OUTPUT", "");
                    sb.AppendLine($"SELECT @{paramName} AS {paramName};");
                }
            }

            return sb.ToString();
        }

        public string GenerateFromTemplate(ScriptTemplate template, TableInfo table)
        {
            var content = template.TemplateContent;

            // Replace common placeholders
            content = content.Replace("{{SchemaName}}", table.SchemaName);
            content = content.Replace("{{TableName}}", table.TableName);
            content = content.Replace("{{DatabaseName}}", table.DatabaseName);

            // Replace column-related placeholders
            var columnsStr = string.Join(",\n", table.Columns.Select(c => $"[{c.Name}]"));
            content = content.Replace("{{Columns}}", columnsStr);

            var parametersStr = string.Join(",\n", table.Columns.Select(c => $"@{c.Name} {GetSqlDataType(c)}"));
            content = content.Replace("{{Parameters}}", parametersStr);

            // Replace custom parameters
            foreach (var param in template.Parameters)
            {
                content = content.Replace($"{{{{{param.Key}}}}}", param.Value);
            }

            return content;
        }

        private string GetSqlDataType(ColumnInfo column)
        {
            var dataType = column.DataType.ToUpper();

            switch (dataType)
            {
                case "VARCHAR":
                case "NVARCHAR":
                case "CHAR":
                case "NCHAR":
                    var length = column.MaxLength == -1 ? "MAX" : column.MaxLength.ToString();
                    return $"{dataType}({length})";

                case "DECIMAL":
                case "NUMERIC":
                    return $"{dataType}({column.Precision},{column.Scale})";

                case "FLOAT":
                    return column.Precision > 0 ? $"{dataType}({column.Precision})" : dataType;

                default:
                    return dataType;
            }
        }
    }
}
