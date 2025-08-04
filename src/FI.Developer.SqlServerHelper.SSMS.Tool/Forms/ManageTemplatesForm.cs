using FI.Developer.SqlServerHelper.Core.Interfaces;
using FI.Developer.SqlServerHelper.Core.Models;

namespace FI.Developer.SqlServerHelper.SSMS.Tool.Forms
{
    public partial class ManageTemplatesForm : Form
    {
        private readonly ITemplateManager _templateManager;
        private List<ScriptTemplate> _templates;
        private ScriptTemplate _currentTemplate;

        public ManageTemplatesForm(ITemplateManager templateManager)
        {
            _templateManager = templateManager;
            InitializeComponent();
            LoadTemplates();
        }

        private async void LoadTemplates()
        {
            try
            {
                _templates = await _templateManager.GetTemplatesAsync();
                lstTemplates.DataSource = _templates;
                lstTemplates.DisplayMember = "Name";
                lstTemplates.ValueMember = "Id";

                if (_templates.Count > 0)
                {
                    lstTemplates.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading templates: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTemplates.SelectedItem is ScriptTemplate template)
            {
                _currentTemplate = template;
                txtName.Text = template.Name;
                txtDescription.Text = template.Description;
                txtContent.Text = template.TemplateContent;
                cboType.SelectedItem = template.Type.ToString();

                // Disable editing for default templates
                bool isDefault = template.Id == "upsert" || template.Id == "delete" || template.Id == "select";
                txtName.ReadOnly = isDefault;
                txtDescription.ReadOnly = isDefault;
                txtContent.ReadOnly = isDefault;
                cboType.Enabled = !isDefault;
                btnSave.Enabled = !isDefault;
                btnDelete.Enabled = !isDefault;
            }
        }

        private async void btnNew_Click(object sender, EventArgs e)
        {
            _currentTemplate = new ScriptTemplate
            {
                Id = Guid.NewGuid().ToString(),
                Name = "New Template",
                Description = "Template description",
                Type = TemplateType.StoredProcedure,
                TemplateContent = "-- Template content\n-- Use placeholders like {{SchemaName}}, {{TableName}}, {{DatabaseName}}, etc."
            };

            txtName.Text = _currentTemplate.Name;
            txtDescription.Text = _currentTemplate.Description;
            txtContent.Text = _currentTemplate.TemplateContent;
            cboType.SelectedItem = _currentTemplate.Type.ToString();

            txtName.ReadOnly = false;
            txtDescription.ReadOnly = false;
            txtContent.ReadOnly = false;
            cboType.Enabled = true;
            btnSave.Enabled = true;
            btnDelete.Enabled = false;

            txtName.Focus();
            txtName.SelectAll();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentTemplate == null) return;

                _currentTemplate.Name = txtName.Text;
                _currentTemplate.Description = txtDescription.Text;
                _currentTemplate.TemplateContent = txtContent.Text;
                _currentTemplate.Type = (TemplateType)Enum.Parse(typeof(TemplateType), cboType.SelectedItem.ToString());

                await _templateManager.SaveTemplateAsync(_currentTemplate);

                MessageBox.Show("Template saved successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadTemplates();

                // Select the saved template
                for (int i = 0; i < lstTemplates.Items.Count; i++)
                {
                    if (((ScriptTemplate)lstTemplates.Items[i]).Id == _currentTemplate.Id)
                    {
                        lstTemplates.SelectedIndex = i;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving template: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentTemplate == null) return;

                var result = MessageBox.Show($"Are you sure you want to delete the template '{_currentTemplate.Name}'?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    await _templateManager.DeleteTemplateAsync(_currentTemplate.Id);
                    LoadTemplates();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting template: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnInsertPlaceholder_Click(object sender, EventArgs e)
        {
            using (var dialog = new PlaceholderDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtContent.SelectedText = dialog.SelectedPlaceholder;
                }
            }
        }
    }
}
