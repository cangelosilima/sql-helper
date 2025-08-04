using FI.Developer.SqlServerHelper.Core.Services;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FI.Developer.SqlServerHelper.SSMS.Tool.Forms
{
    public partial class GenerateExecutionForm : Form
    {
        private readonly string _connectionString;
        private readonly ScriptGeneratorService _scriptGenerator;

        public DataTable ResultData { get; set; }
        public string GeneratedScript { get; private set; }

        public GenerateExecutionForm(string connectionString)
        {
            _connectionString = connectionString;
            _scriptGenerator = new ScriptGeneratorService(new TemplateManager());

            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadProcedures();

            if (ResultData != null && ResultData.Rows.Count > 0)
            {
                dgvData.DataSource = ResultData;
                lblRowCount.Text = $"Rows: {ResultData.Rows.Count}";
                MapColumns();
            }
        }

        private async void LoadProcedures()
        {
            try
            {
                var procedures = new List<string>();

                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"
                        SELECT 
                            SCHEMA_NAME(schema_id) + '.' + name AS ProcedureName
                        FROM sys.procedures
                        ORDER BY SCHEMA_NAME(schema_id), name";

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            procedures.Add(reader.GetString(0));
                        }
                    }
                }

                cboProcedure.DataSource = procedures;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading procedures: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void cboProcedure_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProcedure.SelectedItem == null) return;

            try
            {
                var procedureName = cboProcedure.SelectedItem.ToString();
                var parameters = await GetProcedureParameters(procedureName);

                dgvMapping.Rows.Clear();

                foreach (var param in parameters)
                {
                    var row = dgvMapping.Rows.Add();
                    dgvMapping.Rows[row].Cells["ParameterName"].Value = param.Name;
                    dgvMapping.Rows[row].Cells["DataType"].Value = param.DataType;
                    dgvMapping.Rows[row].Cells["IsOutput"].Value = param.IsOutput;

                    // Try to auto-map columns
                    var columnName = param.Name.TrimStart('@');
                    if (ResultData != null && ResultData.Columns.Contains(columnName))
                    {
                        dgvMapping.Rows[row].Cells["MappedColumn"].Value = columnName;
                    }
                }

                MapColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading procedure parameters: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<List<ParameterInfo>> GetProcedureParameters(string procedureName)
        {
            var parameters = new List<ParameterInfo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var parts = procedureName.Split('.');
                var schema = parts[0];
                var name = parts[1];

                var query = @"
                    SELECT 
                        p.name AS ParameterName,
                        t.name AS DataType,
                        p.max_length,
                        p.precision,
                        p.scale,
                        p.is_output
                    FROM sys.procedures pr
                    INNER JOIN sys.parameters p ON pr.object_id = p.object_id
                    INNER JOIN sys.types t ON p.user_type_id = t.user_type_id
                    WHERE SCHEMA_NAME(pr.schema_id) = @Schema
                        AND pr.name = @Name
                    ORDER BY p.parameter_id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Schema", schema);
                    command.Parameters.AddWithValue("@Name", name);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            parameters.Add(new ParameterInfo
                            {
                                Name = reader.GetString(0),
                                DataType = reader.GetString(1),
                                MaxLength = reader.GetInt16(2),
                                Precision = reader.GetByte(3),
                                Scale = reader.GetByte(4),
                                IsOutput = reader.GetBoolean(5)
                            });
                        }
                    }
                }
            }

            return parameters;
        }

        private void MapColumns()
        {
            if (ResultData == null || dgvMapping.Rows.Count == 0) return;

            // Update the MappedColumn combo box items
            var columnNames = ResultData.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName).ToList();

            columnNames.Insert(0, ""); // Empty option

            var mappedColumnColumn = (DataGridViewComboBoxColumn)dgvMapping.Columns["MappedColumn"];
            mappedColumnColumn.Items.Clear();
            mappedColumnColumn.Items.AddRange(columnNames.ToArray());
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboProcedure.SelectedItem == null)
                {
                    MessageBox.Show("Please select a procedure.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (ResultData == null || ResultData.Rows.Count == 0)
                {
                    MessageBox.Show("No data available to generate scripts.", "Validation",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var scripts = new List<string>();
                var procedureName = cboProcedure.SelectedItem.ToString();

                foreach (DataRow dataRow in ResultData.Rows)
                {
                    var parameters = new Dictionary<string, object>();

                    foreach (DataGridViewRow mappingRow in dgvMapping.Rows)
                    {
                        if (mappingRow.Cells["ParameterName"].Value == null) continue;

                        var paramName = mappingRow.Cells["ParameterName"].Value.ToString().TrimStart('@');
                        var mappedColumn = mappingRow.Cells["MappedColumn"].Value?.ToString();
                        var isOutput = (bool)mappingRow.Cells["IsOutput"].Value;

                        if (isOutput)
                        {
                            parameters[paramName + "_OUTPUT"] = DBNull.Value;
                        }
                        else if (!string.IsNullOrEmpty(mappedColumn) && ResultData.Columns.Contains(mappedColumn))
                        {
                            parameters[paramName] = dataRow[mappedColumn];
                        }
                        else if (chkIncludeNullParams.Checked)
                        {
                            parameters[paramName] = DBNull.Value;
                        }
                    }

                    var script = _scriptGenerator.GenerateExecutionScript(procedureName, parameters);
                    scripts.Add(script);
                }

                GeneratedScript = string.Join("\nGO\n\n", scripts);

                // Show preview
                txtPreview.Text = GeneratedScript;
                tabControl.SelectedTab = tabPreview;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating scripts: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GeneratedScript))
            {
                btnGenerate_Click(sender, e);
            }

            if (!string.IsNullOrEmpty(GeneratedScript))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnImportCSV_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                dialog.Title = "Import CSV File";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ResultData = LoadCsvFile(dialog.FileName);
                        dgvData.DataSource = ResultData;
                        lblRowCount.Text = $"Rows: {ResultData.Rows.Count}";
                        MapColumns();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error importing CSV: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private DataTable LoadCsvFile(string fileName)
        {
            var dt = new DataTable();

            using (var reader = new System.IO.StreamReader(fileName))
            {
                var headers = reader.ReadLine()?.Split(',');
                if (headers == null) return dt;

                foreach (var header in headers)
                {
                    dt.Columns.Add(header.Trim());
                }

                while (!reader.EndOfStream)
                {
                    var values = reader.ReadLine()?.Split(',');
                    if (values != null)
                    {
                        var row = dt.NewRow();
                        for (int i = 0; i < headers.Length && i < values.Length; i++)
                        {
                            row[i] = values[i].Trim();
                        }
                        dt.Rows.Add(row);
                    }
                }
            }

            return dt;
        }

        private class ParameterInfo
        {
            public string Name { get; set; }
            public string DataType { get; set; }
            public short MaxLength { get; set; }
            public byte Precision { get; set; }
            public byte Scale { get; set; }
            public bool IsOutput { get; set; }
        }
    }
}
