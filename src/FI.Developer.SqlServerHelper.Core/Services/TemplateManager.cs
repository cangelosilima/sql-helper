using FI.Developer.SqlServerHelper.Core.Interfaces;
using FI.Developer.SqlServerHelper.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FI.Developer.SqlServerHelper.Core.Services
{
    public class TemplateManager : ITemplateManager
    {
        private readonly string _templatesPath;
        private readonly Dictionary<string, ScriptTemplate> _defaultTemplates;

        public TemplateManager(string templatesPath = null)
        {
            _templatesPath = templatesPath ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "FI.Developer.SqlServerHelper",
                "Templates"
            );

            Directory.CreateDirectory(_templatesPath);
            _defaultTemplates = InitializeDefaultTemplates();
        }

        // Replace usages of File.ReadAllTextAsync with await using and StreamReader for compatibility with .NET Framework or older .NET versions.

        public async Task<List<ScriptTemplate>> GetTemplatesAsync()
        {
            var templates = new List<ScriptTemplate>();

            // Load custom templates from disk
            var files = Directory.GetFiles(_templatesPath, "*.json");
            foreach (var file in files)
            {
                string json;
                using (var reader = new StreamReader(file))
                {
                    json = await reader.ReadToEndAsync();
                }
                var template = JsonSerializer.Deserialize<ScriptTemplate>(json);
                templates.Add(template);
            }

            // Add default templates
            templates.AddRange(_defaultTemplates.Values);

            return templates;
        }

        public async Task<ScriptTemplate> GetTemplateAsync(string id)
        {
            var templateFile = Path.Combine(_templatesPath, $"{id}.json");

            if (File.Exists(templateFile))
            {
                string json;
                using (var reader = new StreamReader(templateFile))
                {
                    json = await reader.ReadToEndAsync();
                }
                return JsonSerializer.Deserialize<ScriptTemplate>(json);
            }

            return _defaultTemplates.ContainsKey(id) ? _defaultTemplates[id] : null;
        }

        public async Task SaveTemplateAsync(ScriptTemplate template)
        {
            var templateFile = Path.Combine(_templatesPath, $"{template.Id}.json");
            var json = JsonSerializer.Serialize(template, new JsonSerializerOptions { WriteIndented = true });
            using (var writer = new StreamWriter(templateFile, false))
            {
                await writer.WriteAsync(json);
            }
        }

        public async Task DeleteTemplateAsync(string id)
        {
            var templateFile = Path.Combine(_templatesPath, $"{id}.json");
            if (File.Exists(templateFile))
            {
                File.Delete(templateFile);
            }
        }

        public Task<ScriptTemplate> LoadDefaultTemplate(string templateName)
        {
            return Task.FromResult(_defaultTemplates.ContainsKey(templateName) ? _defaultTemplates[templateName] : null);
        }

        private Dictionary<string, ScriptTemplate> InitializeDefaultTemplates()
        {
            return new Dictionary<string, ScriptTemplate>
            {
                ["upsert"] = new ScriptTemplate
                {
                    Id = "upsert",
                    Name = "Upsert Stored Procedure",
                    Description = "Creates an insert/update stored procedure",
                    Type = TemplateType.StoredProcedure,
                    TemplateContent = @"CREATE OR ALTER PROCEDURE [{{SchemaName}}].[Sp_{{DatabaseName}}_{{TableName}}_Ins]
{{Parameters}}
AS
BEGIN
SET NOCOUNT ON;
-- Upsert logic here
END"
                },
                ["delete"] = new ScriptTemplate
                {
                    Id = "delete",
                    Name = "Delete Stored Procedure",
                    Description = "Creates a delete stored procedure",
                    Type = TemplateType.StoredProcedure,
                    TemplateContent = @"CREATE OR ALTER PROCEDURE [{{SchemaName}}].[Sp_{{DatabaseName}}_{{TableName}}_Del]
{{PrimaryKeyParameters}}
AS
BEGIN
SET NOCOUNT ON;
DELETE FROM [{{SchemaName}}].[{{TableName}}]
WHERE {{PrimaryKeyConditions}}
END"
                },
                ["select"] = new ScriptTemplate
                {
                    Id = "select",
                    Name = "Select Stored Procedure",
                    Description = "Creates a select stored procedure",
                    Type = TemplateType.StoredProcedure,
                    TemplateContent = @"CREATE OR ALTER PROCEDURE [{{SchemaName}}].[Sp_{{DatabaseName}}_{{TableName}}_Sel]
@PageNumber INT = 1,
@PageSize INT = 50
AS
BEGIN
SET NOCOUNT ON;
    
SELECT {{Columns}}
FROM [{{SchemaName}}].[{{TableName}}]
ORDER BY {{PrimaryKeyColumns}}
OFFSET (@PageNumber - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY;
END"
                }
            };
        }
    }
}
