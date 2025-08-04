namespace FI.Developer.SqlServerHelper.SSMS.Tool.Forms
{
    public partial class PlaceholderDialog : Form
    {
        public string SelectedPlaceholder { get; private set; }

        public PlaceholderDialog()
        {
            InitializeComponent();
            LoadPlaceholders();
        }

        private void LoadPlaceholders()
        {
            var placeholders = new[]
            {
                new { Name = "Schema Name", Value = "{{SchemaName}}" },
                new { Name = "Table Name", Value = "{{TableName}}" },
                new { Name = "Database Name", Value = "{{DatabaseName}}" },
                new { Name = "Columns", Value = "{{Columns}}" },
                new { Name = "Parameters", Value = "{{Parameters}}" },
                new { Name = "Primary Key Columns", Value = "{{PrimaryKeyColumns}}" },
                new { Name = "Primary Key Parameters", Value = "{{PrimaryKeyParameters}}" },
                new { Name = "Primary Key Conditions", Value = "{{PrimaryKeyConditions}}" },
                new { Name = "Update Columns", Value = "{{UpdateColumns}}" },
                new { Name = "Insert Columns", Value = "{{InsertColumns}}" },
                new { Name = "Insert Values", Value = "{{InsertValues}}" }
            };

            lstPlaceholders.DataSource = placeholders;
            lstPlaceholders.DisplayMember = "Name";
            lstPlaceholders.ValueMember = "Value";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (lstPlaceholders.SelectedValue != null)
            {
                SelectedPlaceholder = lstPlaceholders.SelectedValue.ToString();
                DialogResult = DialogResult.OK;
            }
        }

        private void lstPlaceholders_DoubleClick(object sender, EventArgs e)
        {
            btnOK_Click(sender, e);
        }
    }
}
