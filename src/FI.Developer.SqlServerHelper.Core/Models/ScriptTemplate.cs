using System.Collections.Generic;

namespace FI.Developer.SqlServerHelper.Core.Models
{
    public class ScriptTemplate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TemplateContent { get; set; }
        public TemplateType Type { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new();
    }

    public enum TemplateType
    {
        StoredProcedure,
        Trigger,
        Function,
        Script
    }
}
