# Referência Completa do Código

Este projeto contém código extenso demais para incluir em todos os arquivos.
Para obter o código completo, consulte os seguintes artifacts no Claude:

1. **sql-server-helper-solution** - Contém a estrutura principal e os serviços Core
2. **generate-script-form** - Contém o código completo do GenerateScriptForm
3. **manage-templates-form** - Contém o código completo do ManageTemplatesForm
4. **generate-execution-form** - Contém o código completo do GenerateExecutionForm

## Como obter o código completo:

1. Peça ao Claude para mostrar o conteúdo de cada artifact
2. Copie o código para os respectivos arquivos
3. Os arquivos de serviços principais estão em:
   - src/FI.Developer.SqlServerHelper.Core/Services/SqlServerService.cs
   - src/FI.Developer.SqlServerHelper.Core/Services/ScriptGeneratorService.cs
   - src/FI.Developer.SqlServerHelper.Core/Services/TemplateManager.cs
   - src/FI.Developer.SqlServerHelper.CLI/Program.cs (completo)
   - src/FI.Developer.SqlServerHelper.SSMS/AddIn.cs (completo)
   - src/FI.Developer.SqlServerHelper.SSMS/Forms/*.cs (todos os forms)

## Estrutura dos Forms:

Cada form tem dois arquivos:
- NomeForm.cs - Código principal
- NomeForm.Designer.cs - Código do designer (gerado pelo Visual Studio)

Certifique-se de criar ambos os arquivos para cada form.
