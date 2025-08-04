using FI.Developer.SqlServerHelper.Core.Models;
using System.Collections.Generic;

namespace FI.Developer.SqlServerHelper.Core.Interfaces
{
    public interface IScriptGenerator
    {
        public string GenerateUpsertProcedure(TableInfo table);
        public string GenerateDeleteProcedure(TableInfo table);
        public string GenerateExecutionScript(string procedureName, Dictionary<string, object> parameters);
        public string GenerateFromTemplate(ScriptTemplate template, TableInfo table);
    }
}
