using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FI.Developer.SqlServerHelper.Core.Models;
using FI.Developer.SqlServerHelper.Core.Services;
using Xunit;

namespace FI.Developer.SqlServerHelper.Tests
{
    public class TemplateManagerTests
    {
        private static string CreateTempDirectory()
        {
            var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(path);
            return path;
        }

        [Fact]
        public async Task SaveGetDeleteTemplate_Works()
        {
            var dir = CreateTempDirectory();
            var manager = new TemplateManager(dir);

            var template = new ScriptTemplate
            {
                Id = "custom",
                Name = "Custom",
                Description = "desc",
                TemplateContent = "content",
                Type = TemplateType.Script
            };

            await manager.SaveTemplateAsync(template);
            var filePath = Path.Combine(dir, "custom.json");
            Assert.True(File.Exists(filePath));

            var loaded = await manager.GetTemplateAsync("custom");
            Assert.Equal("Custom", loaded.Name);

            var templates = await manager.GetTemplatesAsync();
            Assert.Contains(templates, t => t.Id == "custom");
            Assert.True(templates.Count >= 4);

            var defaultTemplate = await manager.GetTemplateAsync("upsert");
            Assert.NotNull(defaultTemplate);
            var missingTemplate = await manager.GetTemplateAsync("missing");
            Assert.Null(missingTemplate);

            await manager.DeleteTemplateAsync("custom");
            Assert.False(File.Exists(filePath));

            // Call again to cover branch when file doesn't exist
            await manager.DeleteTemplateAsync("custom");
        }

        [Fact]
        public async Task LoadDefaultTemplate_ReturnsExpected()
        {
            var dir = CreateTempDirectory();
            var manager = new TemplateManager(dir);

            var existing = await manager.LoadDefaultTemplate("upsert");
            Assert.NotNull(existing);

            var missing = await manager.LoadDefaultTemplate("unknown");
            Assert.Null(missing);
        }
    }
}
