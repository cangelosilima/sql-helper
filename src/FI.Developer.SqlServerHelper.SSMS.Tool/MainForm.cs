using FI.Developer.SqlServerHelper.Core.Interfaces;
using FI.Developer.SqlServerHelper.Core.Services;
using FI.Developer.SqlServerHelper.SSMS.Tool.Forms;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Diagnostics;

namespace FI.Developer.SqlServerHelper.SSMSTool
{
    public partial class MainForm : Form
    {
        private readonly ITemplateManager _templateManager;
        private string _connectionString;

        public MainForm()
        {
            _templateManager = new TemplateManager();
            InitializeComponent();
            LoadRecentConnections();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            using (var dialog = new ConnectionDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _connectionString = dialog.ConnectionString;
                    lblConnectionStatus.Text = $"Connected to: {dialog.ServerName}\\{dialog.DatabaseName}";
                    EnableFeatures(true);
                    SaveRecentConnection(_connectionString);
                }
            }
        }

        private void btnGenerateScripts_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                MessageBox.Show("Please connect to a database first.", "Connection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var form = new GenerateScriptForm(_connectionString, _templateManager))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (chkOpenInSSMS.Checked)
                    {
                        OpenScriptsInSSMS(form.GeneratedScripts);
                    }
                    else
                    {
                        SaveScriptsToFolder(form.GeneratedScripts);
                    }
                }
            }
        }

        private void btnManageTemplates_Click(object sender, EventArgs e)
        {
            using (var form = new ManageTemplatesForm(_templateManager))
            {
                form.ShowDialog();
            }
        }

        private void btnGenerateExecution_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                MessageBox.Show("Please connect to a database first.", "Connection Required",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var form = new GenerateExecutionForm(_connectionString))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (chkOpenInSSMS.Checked)
                    {
                        OpenScriptInSSMS(form.GeneratedScript, "ExecutionScript.sql");
                    }
                    else
                    {
                        SaveScriptToFile(form.GeneratedScript, "ExecutionScript.sql");
                    }
                }
            }
        }

        private void OpenScriptsInSSMS(List<GeneratedScript> scripts)
        {
            try
            {
                foreach (var script in scripts)
                {
                    OpenScriptInSSMS(script.Script, script.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening SSMS: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenScriptInSSMS(string script, string fileName)
        {
            var tempFile = Path.Combine(Path.GetTempPath(), fileName);
            File.WriteAllText(tempFile, script);

            // Try to find SSMS executable
            var ssmsPath = FindSSMS();
            if (!string.IsNullOrEmpty(ssmsPath))
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = ssmsPath,
                    Arguments = $"-nosplash -e \"{tempFile}\"",
                    UseShellExecute = false
                };

                if (!string.IsNullOrEmpty(_connectionString))
                {
                    var builder = new SqlConnectionStringBuilder(_connectionString);
                    startInfo.Arguments += $" -S \"{builder.DataSource}\" -d \"{builder.InitialCatalog}\"";

                    if (builder.IntegratedSecurity)
                    {
                        startInfo.Arguments += " -E";
                    }
                }

                Process.Start(startInfo);
            }
            else
            {
                // Fallback - try to open with default SQL editor
                Process.Start(tempFile);
            }
        }

        private string FindSSMS()
        {
            // Try to find SSMS in common locations
            var possiblePaths = new[]
            {
                @"C:\Program Files (x86)\Microsoft SQL Server Management Studio 19\Common7\IDE\Ssms.exe",
                @"C:\Program Files\Microsoft SQL Server Management Studio 19\Common7\IDE\Ssms.exe",
                @"C:\Program Files (x86)\Microsoft SQL Server Management Studio 18\Common7\IDE\Ssms.exe",
                @"C:\Program Files\Microsoft SQL Server Management Studio 18\Common7\IDE\Ssms.exe",
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                    return path;
            }

            // Try registry
            try
            {
                using (var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\SQL Server Management Studio"))
                {
                    if (key != null)
                    {
                        var versions = key.GetSubKeyNames();
                        foreach (var version in versions)
                        {
                            using (var versionKey = key.OpenSubKey(version))
                            {
                                var installDir = versionKey?.GetValue("InstallDir") as string;
                                if (!string.IsNullOrEmpty(installDir))
                                {
                                    var ssmsPath = Path.Combine(installDir, "Ssms.exe");
                                    if (File.Exists(ssmsPath))
                                        return ssmsPath;
                                }
                            }
                        }
                    }
                }
            }
            catch { }

            return null;
        }

        private void SaveScriptsToFolder(List<GeneratedScript> scripts)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select folder to save scripts";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var script in scripts)
                    {
                        var filePath = Path.Combine(dialog.SelectedPath, script.FileName);
                        File.WriteAllText(filePath, script.Script);
                    }

                    MessageBox.Show($"Scripts saved to {dialog.SelectedPath}", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void SaveScriptToFile(string script, string fileName)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "SQL Files (*.sql)|*.sql|All Files (*.*)|*.*";
                dialog.FileName = fileName;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(dialog.FileName, script);
                    MessageBox.Show($"Script saved to {dialog.FileName}", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void EnableFeatures(bool enabled)
        {
            btnGenerateScripts.Enabled = enabled;
            btnGenerateExecution.Enabled = enabled;
        }

        private void LoadRecentConnections()
        {
            // Load from settings
        }

        private void SaveRecentConnection(string connectionString)
        {
            // Save to settings
        }
    }
}
