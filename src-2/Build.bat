@echo off
echo ========================================
echo SSMS Query Add-in - Build Script
echo ========================================

set MSBUILD_PATH="C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\MSBuild.exe"
set PROJECT_PATH="SSMSQueryAddin.sln"
set CONFIG=Release

if not exist %MSBUILD_PATH% (
    echo Buscando MSBuild em outras localizacoes...
    set MSBUILD_PATH="C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
)

echo Limpando build anterior...
%MSBUILD_PATH% %PROJECT_PATH% /t:Clean /p:Configuration=%CONFIG%

echo Compilando projeto...
%MSBUILD_PATH% %PROJECT_PATH% /t:Build /p:Configuration=%CONFIG% /p:Platform="Any CPU"

if %ERRORLEVEL% EQU 0 (
    echo ✅ Build concluído com sucesso!
    echo Arquivos gerados em: bin\%CONFIG%\
) else (
    echo ❌ Build falhou com código de erro: %ERRORLEVEL%
    pause
    exit /b %ERRORLEVEL%
)

pause
