namespace FI.Developer.SqlServerHelper.SSMSTool
{
    partial class ConnectionDialog
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblServer;
        private ComboBox txtServer;
        private Label lblDatabase;
        private ComboBox txtDatabase;
        private GroupBox grpAuthentication;
        private RadioButton rbWindowsAuth;
        private RadioButton rbSqlAuth;
        private Label lblUsername;
        private TextBox txtUsername;
        private Label lblPassword;
        private TextBox txtPassword;
        private Button btnTest;
        private Button btnOK;
        private Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblServer = new Label();
            txtServer = new ComboBox();
            lblDatabase = new Label();
            txtDatabase = new ComboBox();
            grpAuthentication = new GroupBox();
            rbWindowsAuth = new RadioButton();
            rbSqlAuth = new RadioButton();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            btnTest = new Button();
            btnOK = new Button();
            btnCancel = new Button();
            grpAuthentication.SuspendLayout();
            SuspendLayout();
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(14, 17);
            lblServer.Margin = new Padding(4, 0, 4, 0);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(42, 15);
            lblServer.TabIndex = 0;
            lblServer.Text = "Server:";
            // 
            // txtServer
            // 
            txtServer.FormattingEnabled = true;
            txtServer.Items.AddRange(new object[] { "localhost", "localhost\\SQLEXPRESS", "(local)", "." });
            txtServer.Location = new Point(93, 14);
            txtServer.Margin = new Padding(4, 3, 4, 3);
            txtServer.Name = "txtServer";
            txtServer.Size = new Size(349, 23);
            txtServer.TabIndex = 1;
            // 
            // lblDatabase
            // 
            lblDatabase.AutoSize = true;
            lblDatabase.Location = new Point(14, 48);
            lblDatabase.Margin = new Padding(4, 0, 4, 0);
            lblDatabase.Name = "lblDatabase";
            lblDatabase.Size = new Size(58, 15);
            lblDatabase.TabIndex = 2;
            lblDatabase.Text = "Database:";
            // 
            // txtDatabase
            // 
            txtDatabase.FormattingEnabled = true;
            txtDatabase.Location = new Point(93, 45);
            txtDatabase.Margin = new Padding(4, 3, 4, 3);
            txtDatabase.Name = "txtDatabase";
            txtDatabase.Size = new Size(349, 23);
            txtDatabase.TabIndex = 3;
            // 
            // grpAuthentication
            // 
            grpAuthentication.Controls.Add(rbWindowsAuth);
            grpAuthentication.Controls.Add(rbSqlAuth);
            grpAuthentication.Controls.Add(lblUsername);
            grpAuthentication.Controls.Add(txtUsername);
            grpAuthentication.Controls.Add(lblPassword);
            grpAuthentication.Controls.Add(txtPassword);
            grpAuthentication.Location = new Point(14, 87);
            grpAuthentication.Margin = new Padding(4, 3, 4, 3);
            grpAuthentication.Name = "grpAuthentication";
            grpAuthentication.Padding = new Padding(4, 3, 4, 3);
            grpAuthentication.Size = new Size(429, 162);
            grpAuthentication.TabIndex = 2;
            grpAuthentication.TabStop = false;
            grpAuthentication.Text = "Authentication";
            // 
            // rbWindowsAuth
            // 
            rbWindowsAuth.AutoSize = true;
            rbWindowsAuth.Checked = true;
            rbWindowsAuth.Location = new Point(18, 29);
            rbWindowsAuth.Margin = new Padding(4, 3, 4, 3);
            rbWindowsAuth.Name = "rbWindowsAuth";
            rbWindowsAuth.Size = new Size(156, 19);
            rbWindowsAuth.TabIndex = 0;
            rbWindowsAuth.TabStop = true;
            rbWindowsAuth.Text = "Windows Authentication";
            rbWindowsAuth.UseVisualStyleBackColor = true;
            rbWindowsAuth.CheckedChanged += OnWindowsAuthChanged;
            // 
            // rbSqlAuth
            // 
            rbSqlAuth.AutoSize = true;
            rbSqlAuth.Location = new Point(18, 55);
            rbSqlAuth.Margin = new Padding(4, 3, 4, 3);
            rbSqlAuth.Name = "rbSqlAuth";
            rbSqlAuth.Size = new Size(163, 19);
            rbSqlAuth.TabIndex = 1;
            rbSqlAuth.Text = "SQL Server Authentication";
            rbSqlAuth.UseVisualStyleBackColor = true;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(18, 90);
            lblUsername.Margin = new Padding(4, 0, 4, 0);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(63, 15);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            txtUsername.Enabled = false;
            txtUsername.Location = new Point(93, 87);
            txtUsername.Margin = new Padding(4, 3, 4, 3);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(314, 23);
            txtUsername.TabIndex = 3;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(18, 120);
            lblPassword.Margin = new Padding(4, 0, 4, 0);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(60, 15);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Password:";
            // 
            // txtPassword
            // 
            txtPassword.Enabled = false;
            txtPassword.Location = new Point(93, 117);
            txtPassword.Margin = new Padding(4, 3, 4, 3);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(314, 23);
            txtPassword.TabIndex = 5;
            // 
            // btnTest
            // 
            btnTest.Location = new Point(167, 265);
            btnTest.Margin = new Padding(4, 3, 4, 3);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(88, 27);
            btnTest.TabIndex = 3;
            btnTest.Text = "Test";
            btnTest.UseVisualStyleBackColor = true;
            btnTest.Click += btnTest_Click;
            // 
            // btnOK
            // 
            btnOK.Location = new Point(261, 265);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(88, 27);
            btnOK.TabIndex = 4;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(356, 265);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 27);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // ConnectionDialog
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(457, 306);
            Controls.Add(lblServer);
            Controls.Add(txtServer);
            Controls.Add(lblDatabase);
            Controls.Add(txtDatabase);
            Controls.Add(grpAuthentication);
            Controls.Add(btnTest);
            Controls.Add(btnOK);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ConnectionDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Connect to SQL Server";
            grpAuthentication.ResumeLayout(false);
            grpAuthentication.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
