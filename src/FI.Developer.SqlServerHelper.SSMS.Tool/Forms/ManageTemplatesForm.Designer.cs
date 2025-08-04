namespace FI.Developer.SqlServerHelper.SSMS.Tool.Forms
{
    partial class ManageTemplatesForm
    {
        private System.ComponentModel.IContainer components = null;
        private ListBox lstTemplates;
        private TextBox txtName;
        private TextBox txtDescription;
        private TextBox txtContent;
        private ComboBox cboType;
        private Button btnNew;
        private Button btnSave;
        private Button btnDelete;
        private Button btnClose;
        private Button btnInsertPlaceholder;
        private Label lblTemplates;
        private Label lblName;
        private Label lblDescription;
        private Label lblType;
        private Label lblContent;
        private SplitContainer splitContainer;

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
            this.lstTemplates = new ListBox();
            this.txtName = new TextBox();
            this.txtDescription = new TextBox();
            this.txtContent = new TextBox();
            this.cboType = new ComboBox();
            this.btnNew = new Button();
            this.btnSave = new Button();
            this.btnDelete = new Button();
            this.btnClose = new Button();
            this.btnInsertPlaceholder = new Button();
            this.lblTemplates = new Label();
            this.lblName = new Label();
            this.lblDescription = new Label();
            this.lblType = new Label();
            this.lblContent = new Label();
            this.splitContainer = new SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();

            // splitContainer
            this.splitContainer.Dock = DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Size = new System.Drawing.Size(800, 600);
            this.splitContainer.SplitterDistance = 250;
            this.splitContainer.TabIndex = 0;

            // Panel1 - Templates List
            // lblTemplates
            this.lblTemplates.AutoSize = true;
            this.lblTemplates.Location = new System.Drawing.Point(12, 12);
            this.lblTemplates.Name = "lblTemplates";
            this.lblTemplates.Size = new System.Drawing.Size(59, 13);
            this.lblTemplates.Text = "Templates:";

            // lstTemplates
            this.lstTemplates.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left) | AnchorStyles.Right)));
            this.lstTemplates.FormattingEnabled = true;
            this.lstTemplates.Location = new System.Drawing.Point(12, 31);
            this.lstTemplates.Name = "lstTemplates";
            this.lstTemplates.Size = new System.Drawing.Size(226, 511);
            this.lstTemplates.TabIndex = 0;
            this.lstTemplates.SelectedIndexChanged += new EventHandler(this.lstTemplates_SelectedIndexChanged);

            // btnNew
            this.btnNew.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Left)));
            this.btnNew.Location = new System.Drawing.Point(12, 555);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 23);
            this.btnNew.TabIndex = 1;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new EventHandler(this.btnNew_Click);

            // Panel2 - Template Details
            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.Text = "Name:";

            // txtName
            this.txtName.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(80, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(454, 20);
            this.txtName.TabIndex = 0;

            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(12, 41);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.Text = "Description:";

            // txtDescription
            this.txtDescription.Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Left)
            | AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(80, 38);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(454, 20);
            this.txtDescription.TabIndex = 1;

            // lblType
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(12, 67);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(34, 13);
            this.lblType.Text = "Type:";

            // cboType
            this.cboType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboType.FormattingEnabled = true;
            this.cboType.Items.AddRange(new object[] {
            "StoredProcedure",
            "Trigger",
            "Function",
            "Script"});
            this.cboType.Location = new System.Drawing.Point(80, 64);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(200, 21);
            this.cboType.TabIndex = 2;

            // lblContent
            this.lblContent.AutoSize = true;
            this.lblContent.Location = new System.Drawing.Point(12, 94);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(90, 13);
            this.lblContent.Text = "Template Content:";

            // btnInsertPlaceholder
            this.btnInsertPlaceholder.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            this.btnInsertPlaceholder.Location = new System.Drawing.Point(414, 89);
            this.btnInsertPlaceholder.Name = "btnInsertPlaceholder";
            this.btnInsertPlaceholder.Size = new System.Drawing.Size(120, 23);
            this.btnInsertPlaceholder.TabIndex = 3;
            this.btnInsertPlaceholder.Text = "Insert Placeholder...";
            this.btnInsertPlaceholder.UseVisualStyleBackColor = true;
            this.btnInsertPlaceholder.Click += new EventHandler(this.btnInsertPlaceholder_Click);

            // txtContent
            this.txtContent.Anchor = ((AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom)
            | AnchorStyles.Left) | AnchorStyles.Right)));
            this.txtContent.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContent.Location = new System.Drawing.Point(12, 118);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = ScrollBars.Both;
            this.txtContent.Size = new System.Drawing.Size(522, 424);
            this.txtContent.TabIndex = 4;
            this.txtContent.WordWrap = false;

            // btnSave
            this.btnSave.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(297, 555);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new EventHandler(this.btnSave_Click);

            // btnDelete
            this.btnDelete.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(378, 555);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);

            // btnClose
            this.btnClose.Anchor = ((AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right)));
            this.btnClose.DialogResult = DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(459, 555);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new EventHandler(this.btnClose_Click);

            // Add controls to panels
            this.splitContainer.Panel1.Controls.Add(this.lblTemplates);
            this.splitContainer.Panel1.Controls.Add(this.lstTemplates);
            this.splitContainer.Panel1.Controls.Add(this.btnNew);

            this.splitContainer.Panel2.Controls.Add(this.lblName);
            this.splitContainer.Panel2.Controls.Add(this.txtName);
            this.splitContainer.Panel2.Controls.Add(this.lblDescription);
            this.splitContainer.Panel2.Controls.Add(this.txtDescription);
            this.splitContainer.Panel2.Controls.Add(this.lblType);
            this.splitContainer.Panel2.Controls.Add(this.cboType);
            this.splitContainer.Panel2.Controls.Add(this.lblContent);
            this.splitContainer.Panel2.Controls.Add(this.btnInsertPlaceholder);
            this.splitContainer.Panel2.Controls.Add(this.txtContent);
            this.splitContainer.Panel2.Controls.Add(this.btnSave);
            this.splitContainer.Panel2.Controls.Add(this.btnDelete);
            this.splitContainer.Panel2.Controls.Add(this.btnClose);

            // ManageTemplatesForm
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.splitContainer);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "ManageTemplatesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Manage Templates";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
