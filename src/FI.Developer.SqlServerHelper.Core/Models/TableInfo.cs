using System.Collections.Generic;

namespace FI.Developer.SqlServerHelper.Core.Models
{
    public class TableInfo
    {
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public string DatabaseName { get; set; }
        public List<ColumnInfo> Columns { get; set; } = new();
        public List<ColumnInfo> PrimaryKeyColumns { get; set; } = new();
    }

    public class ColumnInfo
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public int MaxLength { get; set; }
        public int Precision { get; set; }
        public int Scale { get; set; }
        public bool IsNullable { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsPrimaryKey { get; set; }
        public string DefaultValue { get; set; }
    }
}
