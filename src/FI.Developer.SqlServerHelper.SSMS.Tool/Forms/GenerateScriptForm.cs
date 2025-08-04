using FI.Developer.SqlServerHelper.Core.Interfaces;
using FI.Developer.SqlServerHelper.Core.Models;
using FI.Developer.SqlServerHelper.Core.Services;
using System.Data;

namespace FI.Developer.SqlServerHelper.SSMS.Tool.Forms
{
    public partial class GenerateScriptForm : Form
    {
        private readonly string _connectionString;
        private readonly ITemplateManager _templateManager;
        private readonly IScriptGenerator _scriptGenerator;
        private readonly SqlServerService _sqlService;

        public List<string> SelectedTables { get; set; }
        public List<GeneratedScript> GeneratedScripts { get; private set; }

        public GenerateScriptForm(string connectionString, ITemplateManager templateManager)
        {
            _connectionString = connectionString;
            _templateManager = templateManager;
            _scriptGenerator = new ScriptGeneratorService(_templateManager);
            _sqlService = new SqlServerService(_connectionString);

            InitializeComponent();
            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                // Load schemas
                var tables = await _sqlService.GetTablesAsync();
                var schemas = tables.Select(t => t.SchemaName).Distinct().OrderBy(s => s).ToList();
                cboSchema.DataSource = schemas;

                // Load tables
                await LoadTables();

                // Load templates
                var templates = await _templateManager.GetTemplatesAsync();
                lstTemplates.DataSource = templates;
                lstTemplates.DisplayMember = "Name";
                lstTemplates.ValueMember = "Id";

                // Select default templates
                for (int i = 0; i < lstTemplates.Items.Count; i++)
                {
                    var template = (ScriptTemplate)lstTemplates.Items[i];
                    if (template.Id == "upsert" || template.Id == "delete")
                    {
                        lstTemplates.SetItemChecked(i, true);
                    }
                }

                // Select tables from context
                if (SelectedTables != null && SelectedTables.Any())
                {
                    foreach (var table in SelectedTables)
                    {
                        var parts = table.Split('.');
                        if (parts.Length == 2)
                        {
                            var schema = parts[0];
                            var tableName = parts[1];

                            if (schema == cboSchema.SelectedItem?.ToString())
                            {
                                for (int i = 0; i < lstTables.Items.Count; i++)
                                {
                                    var tableInfo = (TableInfo)lstTables.Items[i];
                                    if (tableInfo.TableName == tableName)
                                    {
                                        lstTables.SetItemChecked(i, true);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadTables()
        {
            var schema = cboSchema.SelectedItem?.ToString();
            var tables = await _sqlService.GetTablesAsync(schema);

            lstTables.DataSource = tables;
            lstTables.DisplayMember = "TableName";
            lstTables.ValueMember = "TableName";
        }

        private async void cboSchema_SelectedIndexChanged(object sender, EventArgs e)
        {
            await LoadTables();
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                GeneratedScripts = new List<GeneratedScript>();

                var selectedTables = lstTables.CheckedItems.Cast<TableInfo>().ToList();
                var selectedTemplates = lstTemplates.CheckedItems.Cast<ScriptTemplate>().ToList();

                if (!selectedTables.Any())
                {
                    MessageBox.Show("Please select at least one table.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!selectedTemplates.Any())
                {
                    MessageBox.Show("Please select at least one template.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                progressBar.Maximum = selectedTables.Count * selectedTemplates.Count;
                progressBar.Value = 0;
                progressBar.Visible = true;

                foreach (var table in selectedTables)
                {
                    foreach (var template in selectedTemplates)
                    {
                        string script = "";
                        string fileName = "";

                        // Use built-in logic for default templates
                        if (template.Id == "upsert")
                        {
                            script = _scriptGenerator.GenerateUpsertProcedure(table);
                            fileName = $"Sp_{table.DatabaseName}_{table.TableName}_Ins.sql";
                        }
                        else if (template.Id == "delete")
                        {
                            script = _scriptGenerator.GenerateDeleteProcedure(table);
                            fileName = $"Sp_{table.DatabaseName}_{table.TableName}_Del.sql";
                        }
                        else
                        {
                            script = _scriptGenerator.GenerateFromTemplate(template, table);
                            fileName = $"{table.SchemaName}.{table.TableName}_{template.Name}.sql";
                        }

                        GeneratedScripts.Add(new GeneratedScript
                        {
                            FileName = fileName,
                            Script = script
                        });

                        progressBar.Value++;
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating scripts: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                progressBar.Visible = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstTables.Items.Count; i++)
            {
                lstTables.SetItemChecked(i, true);
            }
        }

        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lstTables.Items.Count; i++)
            {
                lstTables.SetItemChecked(i, false);
            }
        }
    }

    public class GeneratedScript
    {
        public string FileName { get; set; }
        public string Script { get; set; }
    }
}
