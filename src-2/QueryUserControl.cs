using System;
using System.Drawing;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;

namespace SSMSQueryAddin
{
    public partial class QueryUserControl : UserControl
    {
        private TextBox txtQuery;
        private Button btnExecute;
        private Button btnClear;
        private Label lblInstructions;
        private DTE2 _dte;
        private const string DEFAULT_CONNECTION = "Data Source=localhost;Initial Catalog=master;Integrated Security=True";

        public QueryUserControl(DTE2 dte)
        {
            _dte = dte;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Configurações do UserControl
            this.Name = "QueryUserControl";
            this.Size = new Size(420, 140);
            this.BackColor = SystemColors.Control;
            this.Padding = new Padding(10);

            // Label de instruções
            lblInstructions = new Label();
            lblInstructions.Text = "Digite sua query SQL e clique em 'Nova Query' para criar uma nova janela:";
            lblInstructions.Location = new Point(10, 10);
            lblInstructions.Size = new Size(400, 20);
            lblInstructions.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblInstructions.ForeColor = Color.DarkBlue;

            // TextBox para query
            txtQuery = new TextBox();
            txtQuery.Location = new Point(10, 35);
            txtQuery.Size = new Size(320, 60);
            txtQuery.Multiline = true;
            txtQuery.ScrollBars = ScrollBars.Vertical;
            txtQuery.Text = "SELECT \r\n    name,\r\n    database_id,\r\n    create_date\r\nFROM sys.databases\r\nORDER BY name";
            txtQuery.Font = new Font("Consolas", 9F, FontStyle.Regular);
            txtQuery.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            // Button Nova Query
            btnExecute = new Button();
            btnExecute.Text = "Nova Query";
            btnExecute.Location = new Point(340, 35);
            btnExecute.Size = new Size(75, 30);
            btnExecute.UseVisualStyleBackColor = true;
            btnExecute.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExecute.BackColor = Color.FromArgb(0, 120, 215);
            btnExecute.ForeColor = Color.White;
            btnExecute.FlatStyle = FlatStyle.Flat;
            btnExecute.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExecute.Click += BtnExecute_Click;

            // Button Clear
            btnClear = new Button();
            btnClear.Text = "Limpar";
            btnClear.Location = new Point(340, 70);
            btnClear.Size = new Size(75, 25);
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.Click += BtnClear_Click;

            // Adicionar controles
            this.Controls.Add(lblInstructions);
            this.Controls.Add(txtQuery);
            this.Controls.Add(btnExecute);
            this.Controls.Add(btnClear);

            // Configurar Enter key
            txtQuery.KeyDown += TxtQuery_KeyDown;

            this.ResumeLayout(false);
        }

        private void TxtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                BtnExecute_Click(sender, e);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtQuery.Clear();
            txtQuery.Focus();
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                string queryText = txtQuery.Text.Trim();
                
                if (string.IsNullOrEmpty(queryText))
                {
                    MessageBox.Show("Digite uma query antes de continuar!", "Aviso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuery.Focus();
                    return;
                }

                CreateNewQueryWindow(queryText);
                
                // Fechar a janela pai após criar a query
                Form parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Erro no BtnExecute_Click: {ex.Message}");
                MessageBox.Show($"Erro ao criar nova query: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateNewQueryWindow(string queryText)
        {
            try
            {
                System.Diagnostics.Trace.WriteLine("Tentando criar nova query window");
                
                // Método 1: Tentar usar ServiceCache (SSMS 2016+)
                if (TryCreateQueryUsingServiceCache(queryText))
                {
                    System.Diagnostics.Trace.WriteLine("Query criada usando ServiceCache");
                    return;
                }

                // Método 2: Usar DTE para criar documento
                CreateQueryUsingDTE(queryText);
                System.Diagnostics.Trace.WriteLine("Query criada usando DTE");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Erro ao criar janela de query: {ex.Message}");
                MessageBox.Show($"Erro ao criar janela de query: {ex.Message}", "Erro");
            }
        }

        private bool TryCreateQueryUsingServiceCache(string queryText)
        {
            try
            {
                // Tentar acessar os serviços do SSMS através de reflexão
                var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    if (assembly.FullName.Contains("Microsoft.SqlServer.Management.UI.VSIntegration"))
                    {
                        var serviceCacheType = assembly.GetType("Microsoft.SqlServer.Management.UI.VSIntegration.ServiceCache");
                        if (serviceCacheType != null)
                        {
                            var serviceProviderProperty = serviceCacheType.GetProperty("ServiceProvider");
                            if (serviceProviderProperty != null)
                            {
                                var serviceProvider = serviceProviderProperty.GetValue(null);
                                if (serviceProvider != null)
                                {
                                    // Usar ScriptFactory se disponível
                                    var scriptFactoryType = assembly.GetType("Microsoft.SqlServer.Management.UI.VSIntegration.ScriptFactory");
                                    if (scriptFactoryType != null)
                                    {
                                        var createMethod = scriptFactoryType.GetMethod("CreateNewBlankScript");
                                        if (createMethod != null)
                                        {
                                            // Criar script em branco
                                            createMethod.Invoke(null, new object[] { 0, null, null }); // ScriptType.Sql = 0
                                            
                                            // Inserir texto
                                            System.Threading.Thread.Sleep(500); // Aguardar criação da janela
                                            InsertTextInActiveDocument(queryText);
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Erro no ServiceCache: {ex.Message}");
            }
            return false;
        }

        private void CreateQueryUsingDTE(string queryText)
        {
            try
            {
                // Criar novo documento usando DTE
                Document doc = _dte.ItemOperations.NewFile(@"General\Text File", "Query.sql", "");
                
                if (doc != null)
                {
                    System.Threading.Thread.Sleep(200); // Aguardar carregamento
                    
                    if (doc.Object() != null)
                    {
                        TextDocument textDoc = (TextDocument)doc.Object("");
                        if (textDoc != null)
                        {
                            EditPoint editPoint = textDoc.StartPoint.CreateEditPoint();
                            editPoint.Insert(queryText);
                            
                            MessageBox.Show("Nova janela de query criada com sucesso!\n\n" +
                                "Dica: Conecte-se ao banco de dados usando o Object Explorer.", 
                                "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao criar documento: {ex.Message}");
            }
        }

        private void InsertTextInActiveDocument(string text)
        {
            try
            {
                if (_dte.ActiveDocument != null)
                {
                    var textDoc = _dte.ActiveDocument.Object("") as TextDocument;
                    if (textDoc != null)
                    {
                        EditPoint editPoint = textDoc.StartPoint.CreateEditPoint();
                        editPoint.Insert(text);
                        System.Diagnostics.Trace.WriteLine("Texto inserido no documento ativo");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Erro ao inserir texto: {ex.Message}");
            }
        }
    }
}
