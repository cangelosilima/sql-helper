using FI.Developer.SqlServerHelper.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FI.Developer.SqlServerHelper.Core.Interfaces
{
    public interface ITemplateManager
    {
        public Task<List<ScriptTemplate>> GetTemplatesAsync();
        public Task<ScriptTemplate> GetTemplateAsync(string id);
        public Task SaveTemplateAsync(ScriptTemplate template);
        public Task DeleteTemplateAsync(string id);
        public Task<ScriptTemplate> LoadDefaultTemplate(string templateName);
    }
}
