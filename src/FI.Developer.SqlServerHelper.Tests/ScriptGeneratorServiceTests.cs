using System;
using System.Collections.Generic;
using System.Linq;
using FI.Developer.SqlServerHelper.Core.Interfaces;
using FI.Developer.SqlServerHelper.Core.Models;
using FI.Developer.SqlServerHelper.Core.Services;
using Moq;
using Xunit;

namespace FI.Developer.SqlServerHelper.Tests
{
    public class ScriptGeneratorServiceTests
    {
        private ScriptGeneratorService CreateService() => new ScriptGeneratorService(Mock.Of<ITemplateManager>());

        private TableInfo CreateSampleTable()
        {
            var table = new TableInfo
            {
                SchemaName = "dbo",
                TableName = "MyTable",
                DatabaseName = "MyDb",
                Columns = new List<ColumnInfo>
                {
                    new ColumnInfo { Name = "Id", DataType = "INT", IsIdentity = true, IsPrimaryKey = true, IsNullable = false },
                    new ColumnInfo { Name = "Name", DataType = "NVARCHAR", MaxLength = 100, IsNullable = false },
                    new ColumnInfo { Name = "Description", DataType = "VARCHAR", MaxLength = 50, IsNullable = true },
                    new ColumnInfo { Name = "DecimalValue", DataType = "DECIMAL", Precision = 18, Scale = 2, IsNullable = false },
                    new ColumnInfo { Name = "FloatVal", DataType = "FLOAT", Precision = 8, IsNullable = false },
                    new ColumnInfo { Name = "FloatValNoPrecision", DataType = "FLOAT", Precision = 0, IsNullable = false },
                    new ColumnInfo { Name = "Dt_Atuliz", DataType = "DATETIME", IsNullable = true },
                    new ColumnInfo { Name = "Id_Usu", DataType = "INT", IsNullable = true }
                }
            };

            table.PrimaryKeyColumns.Add(table.Columns.First(c => c.Name == "Id"));
            return table;
        }

        [Fact]
        public void GenerateUpsertProcedure_BuildsExpectedScript()
        {
            var service = CreateService();
            var table = CreateSampleTable();

            var script = service.GenerateUpsertProcedure(table);

            Assert.Contains("CREATE OR ALTER PROCEDURE [dbo].[Sp_MyDb_MyTable_Ins]", script);
            Assert.Contains("@Id INT OUTPUT", script);
            Assert.Contains("@Name NVARCHAR(100)", script);
            Assert.Contains("@Description VARCHAR(50) = NULL", script);
            Assert.Contains("@DecimalValue DECIMAL(18,2)", script);
            Assert.Contains("@FloatVal FLOAT(8)", script);
            Assert.Contains("@FloatValNoPrecision FLOAT", script);
            Assert.Contains("UPDATE [dbo].[MyTable]", script);
            Assert.Contains("[Id] = @Id", script);
            Assert.Contains("INSERT INTO [dbo].[MyTable]", script);
            Assert.Contains("SET @Id = SCOPE_IDENTITY();", script);
        }

        [Fact]
        public void GenerateDeleteProcedure_BuildsExpectedScript()
        {
            var service = CreateService();
            var table = CreateSampleTable();

            var script = service.GenerateDeleteProcedure(table);

            Assert.Contains("CREATE OR ALTER PROCEDURE [dbo].[Sp_MyDb_MyTable_Del]", script);
            Assert.Contains("@Id INT", script);
            Assert.Contains("DELETE FROM [dbo].[MyTable]", script);
            Assert.Contains("[Id] = @Id", script);
        }

        [Fact]
        public void GenerateExecutionScript_BuildsScriptCorrectly()
        {
            var service = CreateService();
            var parameters = new Dictionary<string, object>
            {
                ["Id"] = 1,
                ["Name"] = "John",
                ["Date"] = new DateTime(2023, 1, 1),
                ["Nullable"] = null,
                ["OutputParam_OUTPUT"] = 0
            };

            var script = service.GenerateExecutionScript("dbo.Sp_Test", parameters);

            Assert.Contains("DECLARE @OutputParam INT;", script);
            Assert.Contains("EXEC dbo.Sp_Test", script);
            Assert.Contains("@Id = 1", script);
            Assert.Contains("@Name = 'John'", script);
            Assert.Contains("@Date = '", script);
            Assert.Contains("@Nullable = NULL", script);
            Assert.Contains("@OutputParam = @OutputParam OUTPUT", script);
            Assert.Contains("SELECT @OutputParam AS OutputParam;", script);
        }

        [Fact]
        public void GenerateFromTemplate_ReplacesPlaceholders()
        {
            var service = CreateService();
            var table = new TableInfo
            {
                SchemaName = "dbo",
                TableName = "MyTable",
                DatabaseName = "MyDb",
                Columns = new List<ColumnInfo>
                {
                    new ColumnInfo { Name = "Col1", DataType = "NVARCHAR", MaxLength = -1 },
                    new ColumnInfo { Name = "Col2", DataType = "DECIMAL", Precision = 10, Scale = 2 },
                    new ColumnInfo { Name = "Col3", DataType = "FLOAT", Precision = 5 },
                    new ColumnInfo { Name = "Col4", DataType = "FLOAT", Precision = 0 },
                    new ColumnInfo { Name = "Col5", DataType = "INT" }
                }
            };

            var template = new ScriptTemplate
            {
                TemplateContent = "Schema: {{SchemaName}}, Table: {{TableName}}, DB: {{DatabaseName}}, Cols: {{Columns}}, Params: {{Parameters}}, Custom: {{Custom}}",
                Parameters = new Dictionary<string, string> { ["Custom"] = "Value" }
            };

            var content = service.GenerateFromTemplate(template, table);

            Assert.Contains("Schema: dbo", content);
            Assert.Contains("Table: MyTable", content);
            Assert.Contains("DB: MyDb", content);
            Assert.Contains("[Col1]", content);
            Assert.Contains("@Col2 DECIMAL(10,2)", content);
            Assert.Contains("@Col3 FLOAT(5)", content);
            Assert.Contains("@Col4 FLOAT", content);
            Assert.Contains("@Col5 INT", content);
            Assert.Contains("Custom: Value", content);
        }
    }
}
