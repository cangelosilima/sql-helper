using FI.Developer.SqlServerHelper.Core.Services;
using FI.Developer.SqlServerHelper.SSMS.Tool.Forms;
using FI.Developer.SqlServerHelper.SSMSTool;
using System.Diagnostics;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Se foi chamado com argumentos, processar
        if (args.Length > 0)
        {
            ProcessCommandLineArgs(args);
        }
        else
        {
            // Abrir janela principal
            Application.Run(new MainForm());
        }
    }

    static void ProcessCommandLineArgs(string[] args)
    {
        if (args[0] == "--generate" && args.Length >= 3)
        {
            var connectionString = args[1];
            var tables = args[2].Split(',');

            var templateManager = new TemplateManager();
            using (var form = new GenerateScriptForm(connectionString, templateManager))
            {
                form.SelectedTables = new List<string>(tables);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    OpenScriptsInSSMS(form.GeneratedScripts);
                }
            }
        }
    }

    static void OpenScriptsInSSMS(List<GeneratedScript> scripts)
    {
        foreach (var script in scripts)
        {
            var tempFile = Path.Combine(Path.GetTempPath(), script.FileName);
            File.WriteAllText(tempFile, script.Script);

            // Abrir no SSMS
            Process.Start("ssms.exe", $"-e \"{tempFile}\"");
        }
    }
}
