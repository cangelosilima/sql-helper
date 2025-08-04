using System.Data.SqlClient;

namespace FI.Developer.SqlServerHelper.SSMSTool
{
    public partial class ConnectionDialog : Form
    {
        public string ConnectionString { get; private set; }
        public string ServerName { get; private set; }
        public string DatabaseName { get; private set; }

        public ConnectionDialog()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                var builder = BuildConnectionString();
                using (var connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    MessageBox.Show("Connection successful!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var builder = BuildConnectionString();
            ConnectionString = builder.ConnectionString;
            ServerName = builder.DataSource;
            DatabaseName = builder.InitialCatalog;

            DialogResult = DialogResult.OK;
        }

        private SqlConnectionStringBuilder BuildConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = txtServer.Text,
                InitialCatalog = txtDatabase.Text
            };

            if (rbWindowsAuth.Checked)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.UserID = txtUsername.Text;
                builder.Password = txtPassword.Text;
            }

            return builder;
        }

        private void OnWindowsAuthChanged(object sender, EventArgs e)
        {
            this.rbWindowsAuth.CheckedChanged += (s, e) => {
                txtUsername.Enabled = !rbWindowsAuth.Checked;
                txtPassword.Enabled = !rbWindowsAuth.Checked;
            };
        }
    }
}
