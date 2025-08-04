using System;
using System.Windows.Forms;

namespace FI.Developer.SqlServerHelper.SSMSTool
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private ToolStrip toolStrip;
        private ToolStripButton btnConnect;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton btnGenerateScripts;
        private ToolStripButton btnManageTemplates;
        private ToolStripButton btnGenerateExecution;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblConnectionStatus;
        private CheckBox chkOpenInSSMS;
        private GroupBox grpOptions;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            toolStrip = new ToolStrip();
            btnConnect = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            btnGenerateScripts = new ToolStripButton();
            btnManageTemplates = new ToolStripButton();
            btnGenerateExecution = new ToolStripButton();
            statusStrip = new StatusStrip();
            lblConnectionStatus = new ToolStripStatusLabel();
            grpOptions = new GroupBox();
            chkOpenInSSMS = new CheckBox();
            toolStrip.SuspendLayout();
            statusStrip.SuspendLayout();
            grpOptions.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip
            // 
            toolStrip.Items.AddRange(new ToolStripItem[] { btnConnect, toolStripSeparator1, btnGenerateScripts, btnManageTemplates, btnGenerateExecution });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(700, 25);
            toolStrip.TabIndex = 0;
            // 
            // btnConnect
            // 
            btnConnect.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnConnect.ImageTransparentColor = Color.Magenta;
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(121, 22);
            btnConnect.Text = "Connect to Database";
            btnConnect.Click += btnConnect_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // btnGenerateScripts
            // 
            btnGenerateScripts.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnGenerateScripts.Enabled = false;
            btnGenerateScripts.ImageTransparentColor = Color.Magenta;
            btnGenerateScripts.Name = "btnGenerateScripts";
            btnGenerateScripts.Size = new Size(96, 22);
            btnGenerateScripts.Text = "Generate Scripts";
            btnGenerateScripts.Click += btnGenerateScripts_Click;
            // 
            // btnManageTemplates
            // 
            btnManageTemplates.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnManageTemplates.ImageTransparentColor = Color.Magenta;
            btnManageTemplates.Name = "btnManageTemplates";
            btnManageTemplates.Size = new Size(110, 22);
            btnManageTemplates.Text = "Manage Templates";
            btnManageTemplates.Click += btnManageTemplates_Click;
            // 
            // btnGenerateExecution
            // 
            btnGenerateExecution.DisplayStyle = ToolStripItemDisplayStyle.Text;
            btnGenerateExecution.Enabled = false;
            btnGenerateExecution.ImageTransparentColor = Color.Magenta;
            btnGenerateExecution.Name = "btnGenerateExecution";
            btnGenerateExecution.Size = new Size(146, 22);
            btnGenerateExecution.Text = "Generate Execution Script";
            btnGenerateExecution.Click += btnGenerateExecution_Click;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { lblConnectionStatus });
            statusStrip.Location = new Point(0, 497);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 16, 0);
            statusStrip.Size = new Size(700, 22);
            statusStrip.TabIndex = 1;
            // 
            // lblConnectionStatus
            // 
            lblConnectionStatus.Name = "lblConnectionStatus";
            lblConnectionStatus.Size = new Size(86, 17);
            lblConnectionStatus.Text = "Not connected";
            // 
            // grpOptions
            // 
            grpOptions.Controls.Add(chkOpenInSSMS);
            grpOptions.Location = new Point(14, 36);
            grpOptions.Margin = new Padding(4, 3, 4, 3);
            grpOptions.Name = "grpOptions";
            grpOptions.Padding = new Padding(4, 3, 4, 3);
            grpOptions.Size = new Size(672, 69);
            grpOptions.TabIndex = 2;
            grpOptions.TabStop = false;
            grpOptions.Text = "Options";
            // 
            // chkOpenInSSMS
            // 
            chkOpenInSSMS.AutoSize = true;
            chkOpenInSSMS.Checked = true;
            chkOpenInSSMS.CheckState = CheckState.Checked;
            chkOpenInSSMS.Location = new Point(18, 29);
            chkOpenInSSMS.Margin = new Padding(4, 3, 4, 3);
            chkOpenInSSMS.Name = "chkOpenInSSMS";
            chkOpenInSSMS.Size = new Size(193, 19);
            chkOpenInSSMS.TabIndex = 0;
            chkOpenInSSMS.Text = "Open generated scripts in SSMS";
            chkOpenInSSMS.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 519);
            Controls.Add(grpOptions);
            Controls.Add(statusStrip);
            Controls.Add(toolStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "MainForm";
            Text = "FI SQL Server Helper";
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            grpOptions.ResumeLayout(false);
            grpOptions.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
