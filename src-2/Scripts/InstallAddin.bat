@echo off
echo ========================================
echo SSMS Query Add-in - Script de Instalacao
echo ========================================
echo.

:: Verificar se esta executando como administrador
net session >nul 2>&1
if %errorLevel% == 0 (
    echo Executando como Administrador... OK
) else (
    echo ERRO: Execute como Administrador!
    pause
    exit /b 1
)

:: Definir caminhos
set ADDIN_PATH=%USERPROFILE%\Documents\Visual Studio 2017\Addins\
set SOURCE_PATH=%~dp0..\bin\Release\

echo Pasta de destino: %ADDIN_PATH%
echo Pasta de origem: %SOURCE_PATH%

:: Criar pasta de destino se nao existir
if not exist "%ADDIN_PATH%" (
    echo Criando pasta de add-ins...
    mkdir "%ADDIN_PATH%"
)

:: Copiar arquivos
echo Copiando arquivos...
copy "%SOURCE_PATH%SSMSQueryAddin.dll" "%ADDIN_PATH%" /Y
copy "%SOURCE_PATH%SSMSQueryAddin.AddIn" "%ADDIN_PATH%" /Y
copy "%SOURCE_PATH%SSMSQueryAddin.pdb" "%ADDIN_PATH%" /Y

:: Registrar assembly COM
echo Registrando assembly COM...
regasm "%ADDIN_PATH%SSMSQueryAddin.dll" /codebase

echo.
echo ========================================
echo Instalacao concluida!
echo ========================================
echo.
echo Proximo passo:
echo 1. Feche o SSMS se estiver aberto
echo 2. Abra o SSMS
echo 3. Va em Tools ^> Query Helper
echo.
pause
