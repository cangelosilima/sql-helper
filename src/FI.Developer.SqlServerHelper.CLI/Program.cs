using FI.Developer.SqlServerHelper.Core.Interfaces;
using FI.Developer.SqlServerHelper.Core.Models;
using FI.Developer.SqlServerHelper.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Parsing;

namespace FI.Developer.SqlServerHelper.CLI
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand("SQL Server Helper CLI Tool");

            // Add commands
            rootCommand.AddCommand(CreateGenerateCommand());
            rootCommand.AddCommand(CreateTemplateCommand());
            rootCommand.AddCommand(CreateExecuteCommand());

            var builder = new CommandLineBuilder(rootCommand)
                .UseHost(_ => Host.CreateDefaultBuilder(args), ConfigureHost)
                .UseDefaults();

            var parser = builder.Build();
            return await parser.InvokeAsync(args);
        }

        private static void ConfigureHost(IHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<ITemplateManager, TemplateManager>();
                services.AddTransient<IScriptGenerator, ScriptGeneratorService>();
                services.AddTransient<SqlServerService>();
            });
        }

        private static Command CreateGenerateCommand()
        {
            var command = new Command("generate", "Generate SQL scripts");

            var connectionStringOption = new Option<string>(
                "--connection-string",
                "SQL Server connection string"
            )
            { IsRequired = true };

            var schemaOption = new Option<string>(
                "--schema",
                "Schema name"
            );

            var tableOption = new Option<string>(
                "--table",
                "Table name (if not specified, all tables will be processed)"
            );

            var outputPathOption = new Option<string>(
                "--output",
                getDefaultValue: () => ".",
                "Output directory path"
            );

            var typeOption = new Option<string>(
                "--type",
                getDefaultValue: () => "both",
                "Script type: upsert, delete, or both"
            );

            command.AddOption(connectionStringOption);
            command.AddOption(schemaOption);
            command.AddOption(tableOption);
            command.AddOption(outputPathOption);
            command.AddOption(typeOption);

            command.SetHandler(async (context) =>
            {
                var connectionString = context.ParseResult.GetValueForOption(connectionStringOption);
                var schema = context.ParseResult.GetValueForOption(schemaOption);
                var table = context.ParseResult.GetValueForOption(tableOption);
                var outputPath = context.ParseResult.GetValueForOption(outputPathOption);
                var type = context.ParseResult.GetValueForOption(typeOption);

                var host = context.GetHost();
                var scriptGenerator = host.Services.GetRequiredService<IScriptGenerator>();
                var sqlService = new SqlServerService(connectionString);

                var tables = string.IsNullOrEmpty(table)
                    ? await sqlService.GetTablesAsync(schema)
                    : new List<TableInfo> { await sqlService.GetTableInfoAsync(schema, table) };

                foreach (var tableInfo in tables.Where(t => t != null))
                {
                    if (type == "upsert" || type == "both")
                    {
                        var upsertScript = scriptGenerator.GenerateUpsertProcedure(tableInfo);
                        var upsertFileName = $"Sp_{tableInfo.DatabaseName}_{tableInfo.TableName}_Ins.sql";
                        await File.WriteAllTextAsync(Path.Combine(outputPath, upsertFileName), upsertScript);
                        Console.WriteLine($"Generated: {upsertFileName}");
                    }

                    if (type == "delete" || type == "both")
                    {
                        var deleteScript = scriptGenerator.GenerateDeleteProcedure(tableInfo);
                        var deleteFileName = $"Sp_{tableInfo.DatabaseName}_{tableInfo.TableName}_Del.sql";
                        await File.WriteAllTextAsync(Path.Combine(outputPath, deleteFileName), deleteScript);
                        Console.WriteLine($"Generated: {deleteFileName}");
                    }
                }
            });

            return command;
        }

        private static Command CreateTemplateCommand()
        {
            var command = new Command("template", "Manage script templates");

            var listCommand = new Command("list", "List all templates");
            listCommand.SetHandler(async (context) =>
            {
                var host = context.GetHost();
                var templateManager = host.Services.GetRequiredService<ITemplateManager>();
                var templates = await templateManager.GetTemplatesAsync();

                Console.WriteLine("Available Templates:");
                foreach (var template in templates)
                {
                    Console.WriteLine($"  {template.Id}: {template.Name} - {template.Description}");
                }
            });

            var createCommand = new Command("create", "Create a new template");
            var idOption = new Option<string>("--id", "Template ID") { IsRequired = true };
            var nameOption = new Option<string>("--name", "Template name") { IsRequired = true };
            var fileOption = new Option<string>("--file", "Template file path") { IsRequired = true };

            createCommand.AddOption(idOption);
            createCommand.AddOption(nameOption);
            createCommand.AddOption(fileOption);

            createCommand.SetHandler(async (context) =>
            {
                var host = context.GetHost();
                var templateManager = host.Services.GetRequiredService<ITemplateManager>();

                var id = context.ParseResult.GetValueForOption(idOption);
                var name = context.ParseResult.GetValueForOption(nameOption);
                var file = context.ParseResult.GetValueForOption(fileOption);

                var template = new ScriptTemplate
                {
                    Id = id,
                    Name = name,
                    TemplateContent = await File.ReadAllTextAsync(file),
                    Type = TemplateType.StoredProcedure
                };

                await templateManager.SaveTemplateAsync(template);
                Console.WriteLine($"Template '{name}' created successfully.");
            });

            command.AddCommand(listCommand);
            command.AddCommand(createCommand);

            return command;
        }

        private static Command CreateExecuteCommand()
        {
            var command = new Command("execute", "Generate execution scripts from result grid");

            var connectionStringOption = new Option<string>(
                "--connection-string",
                "SQL Server connection string"
            )
            { IsRequired = true };

            var procedureOption = new Option<string>(
                "--procedure",
                "Stored procedure name"
            )
            { IsRequired = true };

            var csvFileOption = new Option<string>(
                "--csv",
                "CSV file with data"
            )
            { IsRequired = true };

            var outputOption = new Option<string>(
                "--output",
                "Output file path"
            );

            command.AddOption(connectionStringOption);
            command.AddOption(procedureOption);
            command.AddOption(csvFileOption);
            command.AddOption(outputOption);

            command.SetHandler(async (context) =>
            {
                var procedureName = context.ParseResult.GetValueForOption(procedureOption);
                var csvFile = context.ParseResult.GetValueForOption(csvFileOption);
                var outputFile = context.ParseResult.GetValueForOption(outputOption);

                var host = context.GetHost();
                var scriptGenerator = host.Services.GetRequiredService<IScriptGenerator>();

                // Read CSV and generate execution scripts
                var lines = await File.ReadAllLinesAsync(csvFile);
                var headers = lines[0].Split(',').Select(h => h.Trim()).ToArray();
                var scripts = new List<string>();

                for (int i = 1; i < lines.Length; i++)
                {
                    var values = lines[i].Split(',').Select(v => v.Trim()).ToArray();
                    var parameters = new Dictionary<string, object>();

                    for (int j = 0; j < headers.Length && j < values.Length; j++)
                    {
                        parameters[headers[j]] = values[j];
                    }

                    var script = scriptGenerator.GenerateExecutionScript(procedureName, parameters);
                    scripts.Add(script);
                }

                var fullScript = string.Join("\nGO\n\n", scripts);

                if (!string.IsNullOrEmpty(outputFile))
                {
                    await File.WriteAllTextAsync(outputFile, fullScript);
                    Console.WriteLine($"Execution script saved to: {outputFile}");
                }
                else
                {
                    Console.WriteLine(fullScript);
                }
            });

            return command;
        }
    }
}
