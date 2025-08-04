namespace FI.Developer.SqlServerHelper.SSMS.Tool.Forms
{
    partial class GenerateExecutionForm
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabControl;
        private TabPage tabData;
        private TabPage tabMapping;
        private TabPage tabPreview;
        private DataGridView dgvData;
        private DataGridView dgvMapping;
        private TextBox txtPreview;
        private ComboBox cboProcedure;
        private Button btnGenerate;
        private Button btnOK;
        private Button btnCancel;
        private Button btnImportCSV;
        private CheckBox chkIncludeNullParams;
        private Label lblProcedure;
        private Label lblRowCount;

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
            this.tabControl = new TabControl();
            this.tabData = new TabPage();
            this.tabMapping = new TabPage();
            this.tabPreview = new TabPage();
            this.dgvData = new DataGridView();
            this.dgvMapping = new DataGridView();
            this.txtPreview = new TextBox();
            this.cboProcedure = new ComboBox();
            this.btnGenerate = new Button();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.btnImportCSV = new Button();
            this.chkIncludeNullParams = new CheckBox();
            this.lblProcedure = new Label();
            this.lblRowCount = new Label();

            this.tabControl.SuspendLayout();
            this.tabData.SuspendLayout();
            this.tabMapping.SuspendLayout();
            this.tabPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMapping)).BeginInit();
            this.SuspendLayout();

            // lblProcedure
            this.lblProcedure.AutoSize = true;
            this.lblProcedure.Location = new System.Drawing.Point(12, 15);
            this.lblProcedure.Name = "lblProcedure";
            this.lblProcedure.Size = new System.Drawing.Size(59, 13);
            this.lblProcedure.Text = "Procedure:";

            // cboProcedure
            this.cboProcedure.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.cboProcedure.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboProcedure.FormattingEnabled = true;
            this.cboProcedure.Location = new System.Drawing.Point(77, 12);
            this.cboProcedure.Name = "cboProcedure";
            this.cboProcedure.Size = new System.Drawing.Size(595, 21);
            this.cboProcedure.TabIndex = 0;
            this.cboProcedure.SelectedIndexChanged += new EventHandler(this.cboProcedure_SelectedIndexChanged);

            // tabControl
            this.tabControl.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left) | AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabData);
            this.tabControl.Controls.Add(this.tabMapping);
            this.tabControl.Controls.Add(this.tabPreview);
            this.tabControl.Location = new System.Drawing.Point(12, 39);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(760, 400);
            this.tabControl.TabIndex = 1;

            // tabData
            this.tabData.Controls.Add(this.dgvData);
            this.tabData.Controls.Add(this.lblRowCount);
            this.tabData.Controls.Add(this.btnImportCSV);
            this.tabData.Location = new System.Drawing.Point(4, 22);
            this.tabData.Name = "tabData";
            this.tabData.Padding = new Padding(3);
            this.tabData.Size = new System.Drawing.Size(752, 374);
            this.tabData.TabIndex = 0;
            this.tabData.Text = "Data";
            this.tabData.UseVisualStyleBackColor = true;

            // dgvData
            this.dgvData.AllowUserToAddRows = false;
            this.dgvData.AllowUserToDeleteRows = false;
            this.dgvData.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left) | AnchorStyles.Right)));
            this.dgvData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvData.Location = new System.Drawing.Point(6, 35);
            this.dgvData.Name = "dgvData";
            this.dgvData.ReadOnly = true;
            this.dgvData.Size = new System.Drawing.Size(740, 333);
            this.dgvData.TabIndex = 2;

            // lblRowCount
            this.lblRowCount.AutoSize = true;
            this.lblRowCount.Location = new System.Drawing.Point(6, 10);
            this.lblRowCount.Name = "lblRowCount";
            this.lblRowCount.Size = new System.Drawing.Size(46, 13);
            this.lblRowCount.Text = "Rows: 0";

            // btnImportCSV
            this.btnImportCSV.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnImportCSV.Location = new System.Drawing.Point(671, 6);
            this.btnImportCSV.Name = "btnImportCSV";
            this.btnImportCSV.Size = new System.Drawing.Size(75, 23);
            this.btnImportCSV.TabIndex = 0;
            this.btnImportCSV.Text = "Import CSV";
            this.btnImportCSV.UseVisualStyleBackColor = true;
            this.btnImportCSV.Click += new EventHandler(this.btnImportCSV_Click);

            // tabMapping
            this.tabMapping.Controls.Add(this.dgvMapping);
            this.tabMapping.Controls.Add(this.chkIncludeNullParams);
            this.tabMapping.Location = new System.Drawing.Point(4, 22);
            this.tabMapping.Name = "tabMapping";
            this.tabMapping.Padding = new Padding(3);
            this.tabMapping.Size = new System.Drawing.Size(752, 374);
            this.tabMapping.TabIndex = 1;
            this.tabMapping.Text = "Parameter Mapping";
            this.tabMapping.UseVisualStyleBackColor = true;

            // dgvMapping
            this.dgvMapping.AllowUserToAddRows = false;
            this.dgvMapping.AllowUserToDeleteRows = false;
            this.dgvMapping.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left) | AnchorStyles.Right)));
            this.dgvMapping.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMapping.Location = new System.Drawing.Point(6, 29);
            this.dgvMapping.Name = "dgvMapping";
            this.dgvMapping.Size = new System.Drawing.Size(740, 339);
            this.dgvMapping.TabIndex = 1;

            // Setup dgvMapping columns
            this.dgvMapping.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ParameterName",
                HeaderText = "Parameter Name",
                ReadOnly = true,
                Width = 200
            });
            this.dgvMapping.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DataType",
                HeaderText = "Data Type",
                ReadOnly = true,
                Width = 150
            });
            this.dgvMapping.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsOutput",
                HeaderText = "Output",
                ReadOnly = true,
                Width = 60
            });
            this.dgvMapping.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = "MappedColumn",
                HeaderText = "Mapped Column",
                Width = 200,
                FlatStyle = FlatStyle.Flat
            });

            // chkIncludeNullParams
            this.chkIncludeNullParams.AutoSize = true;
            this.chkIncludeNullParams.Checked = true;
            this.chkIncludeNullParams.CheckState = CheckState.Checked;
            this.chkIncludeNullParams.Location = new System.Drawing.Point(6, 6);
            this.chkIncludeNullParams.Name = "chkIncludeNullParams";
            this.chkIncludeNullParams.Size = new System.Drawing.Size(201, 17);
            this.chkIncludeNullParams.TabIndex = 0;
            this.chkIncludeNullParams.Text = "Include unmapped parameters as NULL";
            this.chkIncludeNullParams.UseVisualStyleBackColor = true;

            // tabPreview
            this.tabPreview.Controls.Add(this.txtPreview);
            this.tabPreview.Location = new System.Drawing.Point(4, 22);
            this.tabPreview.Name = "tabPreview";
            this.tabPreview.Padding = new Padding(3);
            this.tabPreview.Size = new System.Drawing.Size(752, 374);
            this.tabPreview.TabIndex = 2;
            this.tabPreview.Text = "Preview";
            this.tabPreview.UseVisualStyleBackColor = true;

            // txtPreview
            this.txtPreview.Dock = DockStyle.Fill;
            this.txtPreview.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPreview.Location = new System.Drawing.Point(3, 3);
            this.txtPreview.Multiline = true;
            this.txtPreview.Name = "txtPreview";
            this.txtPreview.ReadOnly = true;
            this.txtPreview.ScrollBars = ScrollBars.Both;
            this.txtPreview.Size = new System.Drawing.Size(746, 368);
            this.txtPreview.TabIndex = 0;
            this.txtPreview.WordWrap = false;

            // btnGenerate
            this.btnGenerate.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(535, 455);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new EventHandler(this.btnGenerate_Click);

            // btnOK
            this.btnOK.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(616, 455);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);

            // btnCancel
            this.btnCancel.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(697, 455);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);

            // GenerateExecutionForm
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(784, 490);
            this.Controls.Add(this.lblProcedure);
            this.Controls.Add(this.cboProcedure);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "GenerateExecutionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Generate Execution Scripts";
            this.tabControl.ResumeLayout(false);
            this.tabData.ResumeLayout(false);
            this.tabData.PerformLayout();
            this.tabMapping.ResumeLayout(false);
            this.tabMapping.PerformLayout();
            this.tabPreview.ResumeLayout(false);
            this.tabPreview.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMapping)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
