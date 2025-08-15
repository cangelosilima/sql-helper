@echo off
echo ========================================
echo SSMS Query Add-in - Script de Desinstalacao
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

echo Pasta de add-ins: %ADDIN_PATH%

:: Desregistrar assembly COM
echo Desregistrando assembly COM...
regasm "%ADDIN_PATH%SSMSQueryAddin.dll" /unregister

:: Remover arquivos
echo Removendo arquivos...
del "%ADDIN_PATH%SSMSQueryAddin.dll" /Q
del "%ADDIN_PATH%SSMSQueryAddin.AddIn" /Q
del "%ADDIN_PATH%SSMSQueryAddin.pdb" /Q

echo.
echo ========================================
echo Desinstalacao concluida!
echo ========================================
echo.
echo O add-in foi removido do sistema.
echo Reinicie o SSMS para aplicar as mudancas.
echo.
pause
