namespace FI.Developer.SqlServerHelper.SSMS.Tool.Forms
{
    partial class GenerateScriptForm
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cboSchema;
        private CheckedListBox lstTables;
        private CheckedListBox lstTemplates;
        private Button btnGenerate;
        private Button btnCancel;
        private Button btnSelectAll;
        private Button btnDeselectAll;
        private ProgressBar progressBar;
        private Label lblSchema;
        private Label lblTables;
        private Label lblTemplates;
        private GroupBox grpTables;
        private GroupBox grpTemplates;

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
            this.cboSchema = new ComboBox();
            this.lstTables = new CheckedListBox();
            this.lstTemplates = new CheckedListBox();
            this.btnGenerate = new Button();
            this.btnCancel = new Button();
            this.btnSelectAll = new Button();
            this.btnDeselectAll = new Button();
            this.progressBar = new ProgressBar();
            this.lblSchema = new Label();
            this.lblTables = new Label();
            this.lblTemplates = new Label();
            this.grpTables = new GroupBox();
            this.grpTemplates = new GroupBox();
            this.grpTables.SuspendLayout();
            this.grpTemplates.SuspendLayout();
            this.SuspendLayout();

            // lblSchema
            this.lblSchema.AutoSize = true;
            this.lblSchema.Location = new System.Drawing.Point(12, 15);
            this.lblSchema.Name = "lblSchema";
            this.lblSchema.Size = new System.Drawing.Size(49, 13);
            this.lblSchema.Text = "Schema:";

            // cboSchema
            this.cboSchema.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboSchema.FormattingEnabled = true;
            this.cboSchema.Location = new System.Drawing.Point(67, 12);
            this.cboSchema.Name = "cboSchema";
            this.cboSchema.Size = new System.Drawing.Size(200, 21);
            this.cboSchema.SelectedIndexChanged += new EventHandler(this.cboSchema_SelectedIndexChanged);

            // grpTables
            this.grpTables.Controls.Add(this.lstTables);
            this.grpTables.Controls.Add(this.btnSelectAll);
            this.grpTables.Controls.Add(this.btnDeselectAll);
            this.grpTables.Location = new System.Drawing.Point(12, 45);
            this.grpTables.Name = "grpTables";
            this.grpTables.Size = new System.Drawing.Size(350, 300);
            this.grpTables.Text = "Tables";

            // lstTables
            this.lstTables.CheckOnClick = true;
            this.lstTables.FormattingEnabled = true;
            this.lstTables.Location = new System.Drawing.Point(6, 19);
            this.lstTables.Name = "lstTables";
            this.lstTables.Size = new System.Drawing.Size(338, 244);
            this.lstTables.HorizontalScrollbar = true;

            // btnSelectAll
            this.btnSelectAll.Location = new System.Drawing.Point(6, 271);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new EventHandler(this.btnSelectAll_Click);

            // btnDeselectAll
            this.btnDeselectAll.Location = new System.Drawing.Point(87, 271);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(75, 23);
            this.btnDeselectAll.Text = "Deselect All";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Click += new EventHandler(this.btnDeselectAll_Click);

            // grpTemplates
            this.grpTemplates.Controls.Add(this.lstTemplates);
            this.grpTemplates.Location = new System.Drawing.Point(368, 45);
            this.grpTemplates.Name = "grpTemplates";
            this.grpTemplates.Size = new System.Drawing.Size(250, 300);
            this.grpTemplates.Text = "Templates";

            // lstTemplates
            this.lstTemplates.CheckOnClick = true;
            this.lstTemplates.FormattingEnabled = true;
            this.lstTemplates.Location = new System.Drawing.Point(6, 19);
            this.lstTemplates.Name = "lstTemplates";
            this.lstTemplates.Size = new System.Drawing.Size(238, 274);

            // progressBar
            this.progressBar.Location = new System.Drawing.Point(12, 351);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(606, 23);
            this.progressBar.Visible = false;

            // btnGenerate
            this.btnGenerate.Location = new System.Drawing.Point(462, 380);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new EventHandler(this.btnGenerate_Click);

            // btnCancel
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(543, 380);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // GenerateScriptForm
            this.AcceptButton = this.btnGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(630, 415);
            this.Controls.Add(this.lblSchema);
            this.Controls.Add(this.cboSchema);
            this.Controls.Add(this.grpTables);
            this.Controls.Add(this.grpTemplates);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenerateScriptForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Generate SQL Scripts";
            this.grpTables.ResumeLayout(false);
            this.grpTemplates.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
