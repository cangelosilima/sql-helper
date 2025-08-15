# Script para ajudar a extrair o código completo dos artifacts
Write-Host "Este script ajuda a obter o código completo do projeto." -ForegroundColor Green
Write-Host ""
Write-Host "Você precisará copiar o código dos seguintes artifacts do Claude:" -ForegroundColor Yellow
Write-Host ""
Write-Host "1. sql-server-helper-solution - Contém:" -ForegroundColor Cyan
Write-Host "   - SqlServerService.cs"
Write-Host "   - ScriptGeneratorService.cs" 
Write-Host "   - TemplateManager.cs"
Write-Host "   - Program.cs (CLI completo)"
Write-Host "   - AddIn.cs (parcial)"
Write-Host ""
Write-Host "2. generate-script-form - Contém:" -ForegroundColor Cyan
Write-Host "   - GenerateScriptForm.cs"
Write-Host "   - GenerateScriptForm.Designer.cs"
Write-Host ""
Write-Host "3. manage-templates-form - Contém:" -ForegroundColor Cyan
Write-Host "   - ManageTemplatesForm.cs"
Write-Host "   - ManageTemplatesForm.Designer.cs"
Write-Host "   - PlaceholderDialog.cs"
Write-Host "   - PlaceholderDialog.Designer.cs"
Write-Host ""
Write-Host "4. generate-execution-form - Contém:" -ForegroundColor Cyan
Write-Host "   - GenerateExecutionForm.cs"
Write-Host "   - GenerateExecutionForm.Designer.cs"
Write-Host ""
Write-Host "Para cada artifact, peça ao Claude:" -ForegroundColor Magenta
Write-Host '"Mostre o conteúdo completo do artifact [nome-do-artifact]"'
Write-Host ""
Write-Host "Depois copie o código para os respectivos arquivos no projeto." -ForegroundColor Green
