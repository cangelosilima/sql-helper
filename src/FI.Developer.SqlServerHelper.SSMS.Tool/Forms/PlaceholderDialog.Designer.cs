namespace FI.Developer.SqlServerHelper.SSMS.Tool.Forms
{
    partial class PlaceholderDialog
    {
        private System.ComponentModel.IContainer components = null;
        private ListBox lstPlaceholders;
        private Button btnOK;
        private Button btnCancel;
        private Label lblInstructions;

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
            this.lstPlaceholders = new ListBox();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            this.lblInstructions = new Label();
            this.SuspendLayout();

            // lblInstructions
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Location = new System.Drawing.Point(12, 9);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(180, 13);
            this.lblInstructions.Text = "Select a placeholder to insert:";

            // lstPlaceholders
            this.lstPlaceholders.FormattingEnabled = true;
            this.lstPlaceholders.Location = new System.Drawing.Point(12, 28);
            this.lstPlaceholders.Name = "lstPlaceholders";
            this.lstPlaceholders.Size = new System.Drawing.Size(360, 238);
            this.lstPlaceholders.TabIndex = 0;
            this.lstPlaceholders.DoubleClick += new EventHandler(this.lstPlaceholders_DoubleClick);

            // btnOK
            this.btnOK.Location = new System.Drawing.Point(216, 280);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);

            // btnCancel
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(297, 280);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;

            // PlaceholderDialog
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 315);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.lstPlaceholders);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlaceholderDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Insert Placeholder";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
