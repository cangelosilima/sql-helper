# SSMS Query Add-in - Script de Diagnóstico
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "SSMS Query Add-in - Diagnóstico" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Verificar pasta de Add-ins
$addinPath = "$env:USERPROFILE\Documents\Visual Studio 2017\Addins\"
Write-Host "1. Verificando pasta de Add-ins..." -ForegroundColor Yellow
Write-Host "   Caminho: $addinPath"

if (Test-Path $addinPath) {
    Write-Host "   ✅ Pasta existe" -ForegroundColor Green
    
    $files = Get-ChildItem $addinPath -Filter "*SSMS*" -ErrorAction SilentlyContinue
    if ($files) {
        Write-Host "   ✅ Arquivos do add-in encontrados:" -ForegroundColor Green
        foreach ($file in $files) {
            Write-Host "      - $($file.Name)" -ForegroundColor White
        }
    } else {
        Write-Host "   ❌ Nenhum arquivo do add-in encontrado" -ForegroundColor Red
    }
} else {
    Write-Host "   ❌ Pasta não existe" -ForegroundColor Red
}

Write-Host ""

# Verificar registro COM
Write-Host "2. Verificando registro COM..." -ForegroundColor Yellow
$regPath = "HKEY_CLASSES_ROOT\SSMSQueryAddin.Connect"
try {
    $regKey = Get-ItemProperty -Path "Registry::$regPath" -ErrorAction Stop
    Write-Host "   ✅ Assembly registrado no COM" -ForegroundColor Green
} catch {
    Write-Host "   ❌ Assembly NÃO registrado no COM" -ForegroundColor Red
}

Write-Host ""

# Verificar processo SSMS
Write-Host "3. Verificando processo SSMS..." -ForegroundColor Yellow
$ssmsProcess = Get-Process -Name "Ssms" -ErrorAction SilentlyContinue
if ($ssmsProcess) {
    Write-Host "   ✅ SSMS está executando (PID: $($ssmsProcess.Id))" -ForegroundColor Green
} else {
    Write-Host "   ⚠️  SSMS não está executando" -ForegroundColor Yellow
}

Write-Host ""

# Verificar versão do .NET Framework
Write-Host "4. Verificando .NET Framework..." -ForegroundColor Yellow
$dotNetVersion = Get-ItemProperty "HKLM:SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\" -Name Release -ErrorAction SilentlyContinue
if ($dotNetVersion) {
    $version = $dotNetVersion.Release
    if ($version -ge 461808) {
        Write-Host "   ✅ .NET Framework 4.7.2+ instalado (Release: $version)" -ForegroundColor Green
    } else {
        Write-Host "   ❌ .NET Framework 4.7.2+ necessário (Release atual: $version)" -ForegroundColor Red
    }
} else {
    Write-Host "   ❌ Não foi possível verificar a versão do .NET Framework" -ForegroundColor Red
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Diagnóstico completo!" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan

Read-Host "Pressione Enter para continuar"
