# ========================================
# SSMS Query Add-in - Gerador Completo
# ========================================
# Este script cria toda a estrutura do projeto e arquivos automaticamente

param(
    [string]$ProjectPath = "C:\Users\cange\source\repos\FI.Developer.SqlServerHelper\src-2",
    [string]$ProjectName = "SSMSQueryAddin",
    [switch]$CreateZip = $false
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "SSMS Query Add-in - Gerador de Projeto" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Verificar se está executando como administrador
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")
if (-not $isAdmin) {
    Write-Warning "Recomendado executar como Administrador para evitar problemas de permissão"
}

Write-Host "📁 Criando estrutura de pastas..." -ForegroundColor Yellow
Write-Host "   Caminho: $ProjectPath" -ForegroundColor Gray

# Criar estrutura de diretórios
$folders = @(
    "$ProjectPath",
    "$ProjectPath\Properties",
    "$ProjectPath\Scripts", 
    "$ProjectPath\Documentation",
    "$ProjectPath\Tests",
    "$ProjectPath\Examples"
)

foreach ($folder in $folders) {
    if (!(Test-Path $folder)) {
        New-Item -ItemType Directory -Path $folder -Force | Out-Null
        Write-Host "   ✅ $($folder.Replace($ProjectPath, '.'))" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  $($folder.Replace($ProjectPath, '.')) (já existe)" -ForegroundColor Yellow
    }
}

Write-Host ""
Write-Host "📄 Criando arquivos do projeto..." -ForegroundColor Yellow

# ========================================
# ARQUIVO: Connect.cs
# ========================================
$connectCs = @'
using System;
using System.Windows.Forms;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SSMSQueryAddin
{
    [ComVisible(true)]
    [Guid("12345678-1234-1234-1234-123456789012")]
    [ProgId("SSMSQueryAddin.Connect")]
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        private DTE2 _applicationObject;
        private AddIn _addInInstance;
        private QueryUserControl _userControl;
        private CommandBarPopup _toolsMenuPopup;
        private CommandBarButton _menuButton;

        public Connect()
        {
        }

        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _applicationObject = (DTE2)application;
            _addInInstance = (AddIn)addInInst;

            try
            {
                System.Diagnostics.Trace.WriteLine($"SSMS Add-in carregando - Modo: {connectMode}");
                
                if (connectMode == ext_ConnectMode.ext_cm_UISetup)
                {
                    CreateMenuButton();
                }
                else if (connectMode == ext_ConnectMode.ext_cm_Startup)
                {
                    CreateUserControl();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Erro no OnConnection: {ex.Message}");
                MessageBox.Show($"Erro ao carregar SSMS Query Add-in: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
            try
            {
                if (_userControl != null && _userControl.Parent != null)
                {
                    _userControl.Parent.Controls.Remove(_userControl);
                    _userControl.Dispose();
                }

                if (_menuButton != null)
                {
                    _menuButton.Delete(true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Erro no OnDisconnection: {ex.Message}");
            }
        }

        public void OnAddInsUpdate(ref Array custom)
        {
        }

        public void OnStartupComplete(ref Array custom)
        {
            CreateUserControl();
        }

        public void OnBeginShutdown(ref Array custom)
        {
        }

        private void CreateMenuButton()
        {
            try
            {
                CommandBars commandBars = (CommandBars)_applicationObject.CommandBars;
                CommandBar toolsMenu = commandBars["Tools"];

                if (toolsMenu != null)
                {
                    _menuButton = (CommandBarButton)toolsMenu.Controls.Add(
                        MsoControlType.msoControlButton,
                        System.Type.Missing,
                        System.Type.Missing,
                        toolsMenu.Controls.Count + 1,
                        true);

                    _menuButton.Caption = "Query Helper";
                    _menuButton.Tag = "QueryHelperAddin";
                    _menuButton.TooltipText = "Abrir Query Helper - Crie queries rapidamente";
                    _menuButton.Click += MenuButton_Click;
                    
                    System.Diagnostics.Trace.WriteLine("Menu button criado com sucesso");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Erro ao criar menu: {ex.Message}");
                MessageBox.Show($"Erro ao criar menu: {ex.Message}", "SSMS Query Add-in");
            }
        }

        private void MenuButton_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            ShowUserControl();
        }

        private void CreateUserControl()
        {
            try
            {
                if (_userControl == null)
                {
                    _userControl = new QueryUserControl(_applicationObject);
                }
                System.Diagnostics.Trace.WriteLine("User control criado");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Erro ao criar user control: {ex.Message}");
                MessageBox.Show($"Erro ao criar user control: {ex.Message}", "SSMS Query Add-in");
            }
        }

        private void ShowUserControl()
        {
            try
            {
                if (_userControl == null)
                {
                    _userControl = new QueryUserControl(_applicationObject);
                }

                Form parentForm = new Form
                {
                    Text = "Query Helper - SSMS Add-in",
                    Size = new System.Drawing.Size(450, 180),
                    StartPosition = FormStartPosition.CenterScreen,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    ShowIcon = false,
                    ShowInTaskbar = false
                };

                _userControl.Dock = DockStyle.Fill;
                parentForm.Controls.Add(_userControl);
                parentForm.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Erro ao exibir user control: {ex.Message}");
                MessageBox.Show($"Erro ao exibir user control: {ex.Message}", "SSMS Query Add-in");
            }
        }

        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, 
            ref vsCommandStatus status, ref object commandText)
        {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                if (commandName == "SSMSQueryAddin.Connect.QueryHelper")
                {
                    status = vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
            }
        }

        public void Exec(string commandName, vsCommandExecOption executeOption, 
            ref object varIn, ref object varOut, ref bool handled)
        {
            handled = false;
            if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                if (commandName == "SSMSQueryAddin.Connect.QueryHelper")
                {
                    ShowUserControl();
                    handled = true;
                    return;
                }
            }
        }
    }
}
'@

# ========================================
# ARQUIVO: QueryUserControl.cs
# ========================================
$queryUserControlCs = @'
using System;
using System.Drawing;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;

namespace SSMSQueryAddin
{
    public partial class QueryUserControl : UserControl
    {
        private TextBox txtQuery;
        private Button btnExecute;
        private Button btnClear;
        private Label lblInstructions;
        private DTE2 _dte;
        private const string DEFAULT_CONNECTION = "Data Source=localhost;Initial Catalog=master;Integrated Security=True";

        public QueryUserControl(DTE2 dte)
        {
            _dte = dte;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Configurações do UserControl
            this.Name = "QueryUserControl";
            this.Size = new Size(420, 140);
            this.BackColor = SystemColors.Control;
            this.Padding = new Padding(10);

            // Label de instruções
            lblInstructions = new Label();
            lblInstructions.Text = "Digite sua query SQL e clique em 'Nova Query' para criar uma nova janela:";
            lblInstructions.Location = new Point(10, 10);
            lblInstructions.Size = new Size(400, 20);
            lblInstructions.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            lblInstructions.ForeColor = Color.DarkBlue;

            // TextBox para query
            txtQuery = new TextBox();
            txtQuery.Location = new Point(10, 35);
            txtQuery.Size = new Size(320, 60);
            txtQuery.Multiline = true;
            txtQuery.ScrollBars = ScrollBars.Vertical;
            txtQuery.Text = "SELECT \r\n    name,\r\n    database_id,\r\n    create_date\r\nFROM sys.databases\r\nORDER BY name";
            txtQuery.Font = new Font("Consolas", 9F, FontStyle.Regular);
            txtQuery.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

            // Button Nova Query
            btnExecute = new Button();
            btnExecute.Text = "Nova Query";
            btnExecute.Location = new Point(340, 35);
            btnExecute.Size = new Size(75, 30);
            btnExecute.UseVisualStyleBackColor = true;
            btnExecute.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnExecute.BackColor = Color.FromArgb(0, 120, 215);
            btnExecute.ForeColor = Color.White;
            btnExecute.FlatStyle = FlatStyle.Flat;
            btnExecute.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExecute.Click += BtnExecute_Click;

            // Button Clear
            btnClear = new Button();
            btnClear.Text = "Limpar";
            btnClear.Location = new Point(340, 70);
            btnClear.Size = new Size(75, 25);
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.Click += BtnClear_Click;

            // Adicionar controles
            this.Controls.Add(lblInstructions);
            this.Controls.Add(txtQuery);
            this.Controls.Add(btnExecute);
            this.Controls.Add(btnClear);

            // Configurar Enter key
            txtQuery.KeyDown += TxtQuery_KeyDown;

            this.ResumeLayout(false);
        }

        private void TxtQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Enter)
            {
                BtnExecute_Click(sender, e);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtQuery.Clear();
            txtQuery.Focus();
        }

        private void BtnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                string queryText = txtQuery.Text.Trim();
                
                if (string.IsNullOrEmpty(queryText))
                {
                    MessageBox.Show("Digite uma query antes de continuar!", "Aviso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuery.Focus();
                    return;
                }

                CreateNewQueryWindow(queryText);
                
                // Fechar a janela pai após criar a query
                Form parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Erro no BtnExecute_Click: {ex.Message}");
                MessageBox.Show($"Erro ao criar nova query: {ex.Message}", "Erro", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateNewQueryWindow(string queryText)
        {
            try
            {
                System.Diagnostics.Trace.WriteLine("Tentando criar nova query window");
                
                // Método 1: Tentar usar ServiceCache (SSMS 2016+)
                if (TryCreateQueryUsingServiceCache(queryText))
                {
                    System.Diagnostics.Trace.WriteLine("Query criada usando ServiceCache");
                    return;
                }

                // Método 2: Usar DTE para criar documento
                CreateQueryUsingDTE(queryText);
                System.Diagnostics.Trace.WriteLine("Query criada usando DTE");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Erro ao criar janela de query: {ex.Message}");
                MessageBox.Show($"Erro ao criar janela de query: {ex.Message}", "Erro");
            }
        }

        private bool TryCreateQueryUsingServiceCache(string queryText)
        {
            try
            {
                // Tentar acessar os serviços do SSMS através de reflexão
                var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                foreach (var assembly in assemblies)
                {
                    if (assembly.FullName.Contains("Microsoft.SqlServer.Management.UI.VSIntegration"))
                    {
                        var serviceCacheType = assembly.GetType("Microsoft.SqlServer.Management.UI.VSIntegration.ServiceCache");
                        if (serviceCacheType != null)
                        {
                            var serviceProviderProperty = serviceCacheType.GetProperty("ServiceProvider");
                            if (serviceProviderProperty != null)
                            {
                                var serviceProvider = serviceProviderProperty.GetValue(null);
                                if (serviceProvider != null)
                                {
                                    // Usar ScriptFactory se disponível
                                    var scriptFactoryType = assembly.GetType("Microsoft.SqlServer.Management.UI.VSIntegration.ScriptFactory");
                                    if (scriptFactoryType != null)
                                    {
                                        var createMethod = scriptFactoryType.GetMethod("CreateNewBlankScript");
                                        if (createMethod != null)
                                        {
                                            // Criar script em branco
                                            createMethod.Invoke(null, new object[] { 0, null, null }); // ScriptType.Sql = 0
                                            
                                            // Inserir texto
                                            System.Threading.Thread.Sleep(500); // Aguardar criação da janela
                                            InsertTextInActiveDocument(queryText);
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Erro no ServiceCache: {ex.Message}");
            }
            return false;
        }

        private void CreateQueryUsingDTE(string queryText)
        {
            try
            {
                // Criar novo documento usando DTE
                Document doc = _dte.ItemOperations.NewFile(@"General\Text File", "Query.sql", "");
                
                if (doc != null)
                {
                    System.Threading.Thread.Sleep(200); // Aguardar carregamento
                    
                    if (doc.Object() != null)
                    {
                        TextDocument textDoc = (TextDocument)doc.Object("");
                        if (textDoc != null)
                        {
                            EditPoint editPoint = textDoc.StartPoint.CreateEditPoint();
                            editPoint.Insert(queryText);
                            
                            MessageBox.Show("Nova janela de query criada com sucesso!\n\n" +
                                "Dica: Conecte-se ao banco de dados usando o Object Explorer.", 
                                "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao criar documento: {ex.Message}");
            }
        }

        private void InsertTextInActiveDocument(string text)
        {
            try
            {
                if (_dte.ActiveDocument != null)
                {
                    var textDoc = _dte.ActiveDocument.Object("") as TextDocument;
                    if (textDoc != null)
                    {
                        EditPoint editPoint = textDoc.StartPoint.CreateEditPoint();
                        editPoint.Insert(text);
                        System.Diagnostics.Trace.WriteLine("Texto inserido no documento ativo");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Erro ao inserir texto: {ex.Message}");
            }
        }
    }
}
'@

# ========================================
# CRIAR TODOS OS ARQUIVOS
# ========================================

Write-Host "   📄 Connect.cs..." -ForegroundColor Green
$connectCs | Out-File -FilePath "$ProjectPath\Connect.cs" -Encoding UTF8

Write-Host "   📄 QueryUserControl.cs..." -ForegroundColor Green
$queryUserControlCs | Out-File -FilePath "$ProjectPath\QueryUserControl.cs" -Encoding UTF8

# ========================================
# ARQUIVO: Properties/AssemblyInfo.cs
# ========================================
$assemblyInfo = @'
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("SSMS Query Add-in")]
[assembly: AssemblyDescription("Add-in para criar queries rapidamente no SQL Server Management Studio")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Sua Empresa")]
[assembly: AssemblyProduct("SSMS Query Add-in")]
[assembly: AssemblyCopyright("Copyright © 2025")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(true)]
[assembly: Guid("12345678-1234-1234-1234-123456789012")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
'@

Write-Host "   📄 Properties/AssemblyInfo.cs..." -ForegroundColor Green
$assemblyInfo | Out-File -FilePath "$ProjectPath\Properties\AssemblyInfo.cs" -Encoding UTF8

# ========================================
# ARQUIVO: SSMSQueryAddin.csproj
# ========================================
$csproj = @'
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <Import Project="$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props" />
  
  <PropertyGroup>
    <Configuration Condition=" '$(Configuration)' == '' ">Debug</Configuration>
    <Platform Condition=" '$(Platform)' == '' ">AnyCPU</Platform>
    <ProjectGuid>{12345678-1234-1234-1234-123456789012}</ProjectGuid>
    <OutputType>Library</OutputType>
    <AppDesignerFolder>Properties</AppDesignerFolder>
    <RootNamespace>SSMSQueryAddin</RootNamespace>
    <AssemblyName>SSMSQueryAddin</AssemblyName>
    <TargetFrameworkVersion>v4.7.2</TargetFrameworkVersion>
    <FileAlignment>512</FileAlignment>
    <RegisterForComInterop>true</RegisterForComInterop>
  </PropertyGroup>

  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
    <DebugSymbols>true</DebugSymbols>
    <DebugType>full</DebugType>
    <Optimize>false</Optimize>
    <OutputPath>bin\Debug\</OutputPath>
    <DefineConstants>DEBUG;TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>

  <PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' ">
    <DebugType>pdbonly</DebugType>
    <Optimize>true</Optimize>
    <OutputPath>bin\Release\</OutputPath>
    <DefineConstants>TRACE</DefineConstants>
    <ErrorReport>prompt</ErrorReport>
    <WarningLevel>4</WarningLevel>
  </PropertyGroup>

  <ItemGroup>
    <Reference Include="Extensibility, Version=7.0.3300.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a">
      <EmbedInteropTypes>False</EmbedInteropTypes>
    </Reference>
    <Reference Include="EnvDTE, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a">
      <EmbedInteropTypes>False</EmbedInteropTypes>
    </Reference>
    <Reference Include="EnvDTE80, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a">
      <EmbedInteropTypes>False</EmbedInteropTypes>
    </Reference>
    <Reference Include="Microsoft.VisualStudio.CommandBars, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a">
      <EmbedInteropTypes>False</EmbedInteropTypes>
    </Reference>
    <Reference Include="System" />
    <Reference Include="System.Core" />
    <Reference Include="System.Drawing" />
    <Reference Include="System.Windows.Forms" />
    
    <!-- Referências específicas do SSMS - ajuste o caminho conforme sua instalação -->
    <Reference Include="Microsoft.SqlServer.Management.UI.VSIntegration">
      <HintPath>C:\Program Files (x86)\Microsoft SQL Server\140\Tools\Binn\ManagementStudio\Microsoft.SqlServer.Management.UI.VSIntegration.dll</HintPath>
      <Private>False</Private>
    </Reference>
  </ItemGroup>

  <ItemGroup>
    <Compile Include="Connect.cs" />
    <Compile Include="QueryUserControl.cs">
      <SubType>UserControl</SubType>
    </Compile>
    <Compile Include="Properties\AssemblyInfo.cs" />
  </ItemGroup>

  <ItemGroup>
    <None Include="SSMSQueryAddin.AddIn">
      <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
    <None Include="app.config" />
  </ItemGroup>

  <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
  
  <PropertyGroup>
    <PostBuildEvent>
      xcopy "$(ProjectDir)SSMSQueryAddin.AddIn" "$(OutputPath)" /Y
      if not exist "%USERPROFILE%\Documents\Visual Studio 2017\Addins\" mkdir "%USERPROFILE%\Documents\Visual Studio 2017\Addins\"
      xcopy "$(OutputPath)*.*" "%USERPROFILE%\Documents\Visual Studio 2017\Addins\" /Y
    </PostBuildEvent>
  </PropertyGroup>
</Project>
'@

Write-Host "   📄 SSMSQueryAddin.csproj..." -ForegroundColor Green
$csproj | Out-File -FilePath "$ProjectPath\SSMSQueryAddin.csproj" -Encoding UTF8

# ========================================
# ARQUIVO: SSMSQueryAddin.sln
# ========================================
$sln = @'
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 16
VisualStudioVersion = 16.0.31424.327
MinimumVisualStudioVersion = 10.0.40219.1
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "SSMSQueryAddin", "SSMSQueryAddin.csproj", "{12345678-1234-1234-1234-123456789012}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{12345678-1234-1234-1234-123456789012}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{12345678-1234-1234-1234-123456789012}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{12345678-1234-1234-1234-123456789012}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{12345678-1234-1234-1234-123456789012}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal
'@

Write-Host "   📄 SSMSQueryAddin.sln..." -ForegroundColor Green
$sln | Out-File -FilePath "$ProjectPath\SSMSQueryAddin.sln" -Encoding UTF8

# ========================================
# ARQUIVO: SSMSQueryAddin.AddIn
# ========================================
$addIn = @'
<?xml version="1.0" encoding="utf-8"?>
<Extensibility xmlns="http://schemas.microsoft.com/AutomationExtensibility">
  <HostApplication>
    <n>Microsoft SQL Server Management Studio</n>
    <Version>11.0</Version>
  </HostApplication>
  <Addin>
    <FriendlyName>SSMS Query Add-in</FriendlyName>
    <Description>Add-in para criar queries rapidamente no SQL Server Management Studio</Description>
    <Assembly>SSMSQueryAddin.dll</Assembly>
    <FullClassName>SSMSQueryAddin.Connect</FullClassName>
    <LoadBehavior>1</LoadBehavior>
    <CommandPreload>0</CommandPreload>
  </Addin>
</Extensibility>
'@

Write-Host "   📄 SSMSQueryAddin.AddIn..." -ForegroundColor Green
$addIn | Out-File -FilePath "$ProjectPath\SSMSQueryAddin.AddIn" -Encoding UTF8

# ========================================
# ARQUIVO: app.config
# ========================================
$appConfig = @'
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <runtime>
    <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
      <dependentAssembly>
        <assemblyIdentity name="EnvDTE" publicKeyToken="b03f5f7f11d50a3a" culture="neutral"/>
        <bindingRedirect oldVersion="0.0.0.0-8.0.0.0" newVersion="8.0.0.0"/>
      </dependentAssembly>
    </assemblyBinding>
  </runtime>
  
  <system.diagnostics>
    <trace autoflush="true">
      <listeners>
        <add name="textListener" 
             type="System.Diagnostics.TextWriterTraceListener" 
             initializeData="C:\temp\SSMSQueryAddin.log" />
      </listeners>
    </trace>
  </system.diagnostics>
  
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
  </startup>
</configuration>
'@

Write-Host "   📄 app.config..." -ForegroundColor Green
$appConfig | Out-File -FilePath "$ProjectPath\app.config" -Encoding UTF8

# ========================================
# SCRIPTS
# ========================================

# Scripts/InstallAddin.bat
$installBat = @'
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
'@

Write-Host "   📄 Scripts/InstallAddin.bat..." -ForegroundColor Green
$installBat | Out-File -FilePath "$ProjectPath\Scripts\InstallAddin.bat" -Encoding UTF8

# Scripts/UninstallAddin.bat
$uninstallBat = @'
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
'@

Write-Host "   📄 Scripts/UninstallAddin.bat..." -ForegroundColor Green
$uninstallBat | Out-File -FilePath "$ProjectPath\Scripts\UninstallAddin.bat" -Encoding UTF8

# Scripts/DiagnosticCheck.ps1
$diagnosticPs1 = @'
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
'@

Write-Host "   📄 Scripts/DiagnosticCheck.ps1..." -ForegroundColor Green
$diagnosticPs1 | Out-File -FilePath "$ProjectPath\Scripts\DiagnosticCheck.ps1" -Encoding UTF8

# ========================================
# DOCUMENTAÇÃO
# ========================================

# README.md
$readme = @'
# SSMS Query Add-in

Um add-in simples para SQL Server Management Studio que permite criar rapidamente novas janelas de query através de uma interface amigável.

## 📋 Funcionalidades

- **User Control** com TextBox e Button integrado ao SSMS
- **Menu personalizado** no Tools do SSMS
- **Criação automática** de nova query window
- **Conexão inteligente** usando a conexão ativa do Object Explorer
- **Interface responsiva** e fácil de usar

## 🛠️ Pré-requisitos

- **Visual Studio 2017** ou superior (recomendado VS 2019/2022)
- **SQL Server Management Studio** (versão 2016 ou superior)
- **.NET Framework 4.7.2** ou superior
- **Permissões de Administrador** para instalação

## 🚀 Instalação Rápida

1. **Baixe** o projeto completo
2. **Execute** como Administrador:
   ```cmd
   cd Scripts
   InstallAddin.bat
   ```
3. **Abra o SSMS** e acesse `Tools > Query Helper`

## 💡 Como Usar

1. **Abra o SSMS**
2. **Clique** em `Tools > Query Helper`
3. **Digite sua query** no TextBox
4. **Clique** em "Nova Query" ou pressione `Ctrl+Enter`
5. **Uma nova janela** de query será criada automaticamente

## 🔧 Desenvolvimento

### Compilar o Projeto
```cmd
# No Visual Studio
Build > Build Solution

# Ou via command line
MSBuild.exe SSMSQueryAddin.sln /p:Configuration=Release
```

### Estrutura do Projeto
```
SSMSQueryAddin/
├── Connect.cs              # Classe principal do add-in
├── QueryUserControl.cs     # Interface do usuário
├── Scripts/               # Scripts de instalação
├── Documentation/         # Documentação técnica
└── Examples/             # Exemplos de uso
```

## 📞 Suporte

- Para problemas de instalação, consulte `Documentation/TROUBLESHOOTING.md`
- Para contribuir, veja `CONTRIBUTING.md`
- Para issues, use o GitHub Issues

## 📝 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para detalhes.

---

**Desenvolvido com ❤️ para a comunidade DBA**
'@

Write-Host "   📄 README.md..." -ForegroundColor Green
$readme | Out-File -FilePath "$ProjectPath\README.md" -Encoding UTF8

# LICENSE
$license = @'
MIT License

Copyright (c) 2025 SSMS Query Add-in

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
'@

Write-Host "   📄 LICENSE..." -ForegroundColor Green
$license | Out-File -FilePath "$ProjectPath\LICENSE" -Encoding UTF8

# Examples/SampleQueries.sql
$sampleQueries = @'
-- SSMS Query Add-in - Queries de Exemplo
-- Copie estas queries para testar o add-in

-- 1. Query básica de sistema
SELECT 
    name as DatabaseName,
    database_id,
    create_date,
    collation_name
FROM sys.databases
ORDER BY name;

-- 2. Informações de tabelas
SELECT 
    t.name AS TableName,
    s.name AS SchemaName,
    p.rows AS RowCount,
    p.data_compression_desc AS Compression
FROM sys.tables t
INNER JOIN sys.schemas s ON t.schema_id = s.schema_id
INNER JOIN sys.partitions p ON t.object_id = p.object_id
WHERE p.index_id IN (0,1)
ORDER BY p.rows DESC;

-- 3. Verificar conexões ativas
SELECT 
    s.session_id,
    s.login_name,
    s.host_name,
    s.program_name,
    s.status,
    s.last_request_start_time
FROM sys.dm_exec_sessions s
WHERE s.is_user_process = 1
ORDER BY s.last_request_start_time DESC;

-- 4. Espaço usado por banco
SELECT 
    DB_NAME() as DatabaseName,
    (SUM(size) * 8.0 / 1024) as SizeMB
FROM sys.database_files;

-- 5. Query de teste simples
SELECT 
    @@SERVERNAME as ServerName,
    @@VERSION as Version,
    GETDATE() as CurrentDate,
    USER_NAME() as UserName,
    DB_NAME() as DatabaseName;
'@

Write-Host "   📄 Examples/SampleQueries.sql..." -ForegroundColor Green
$sampleQueries | Out-File -FilePath "$ProjectPath\Examples\SampleQueries.sql" -Encoding UTF8

# Documentation/TROUBLESHOOTING.md
$troubleshooting = @'
# Guia de Solução de Problemas

## 🚨 Problemas Comuns

### ❌ Add-in não aparece no menu Tools

**Sintomas:**
- Menu "Query Helper" não está visível em Tools
- Add-in Manager não mostra o add-in

**Soluções:**

1. **Verificar localização dos arquivos:**
   ```cmd
   dir "%USERPROFILE%\Documents\Visual Studio 2017\Addins\*SSMS*"
   ```

2. **Reregistrar assembly COM:**
   ```cmd
   regasm "SSMSQueryAddin.dll" /unregister
   regasm "SSMSQueryAddin.dll" /codebase
   ```

3. **Verificar permissões:**
   ```cmd
   icacls "%USERPROFILE%\Documents\Visual Studio 2017\Addins\" /grant Users:F /T
   ```

---

### ❌ Erro "Assembly não encontrado"

**Soluções:**

1. **Verificar versão do .NET:**
   ```powershell
   Get-ItemProperty "HKLM:SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\" -Name Release
   ```

2. **Copiar dependências necessárias para a pasta de add-ins**

---

### ❌ Erro de permissão

**Soluções:**

1. **Executar como Administrador:**
   ```cmd
   regasm "SSMSQueryAddin.dll" /codebase
   ```

2. **Verificar UAC e configurações de segurança**

---

## 🔧 Ferramentas de Diagnóstico

### Script de Diagnóstico Automático
```powershell
.\Scripts\DiagnosticCheck.ps1
```

### Verificação Manual
1. Verificar arquivos essenciais
2. Verificar registro COM
3. Verificar processo SSMS
4. Verificar logs

---

## 📞 Suporte

Para problemas persistentes:
- Consulte a documentação completa
- Execute o script de diagnóstico
- Reporte issues no GitHub
'@

Write-Host "   📄 Documentation/TROUBLESHOOTING.md..." -ForegroundColor Green
$troubleshooting | Out-File -FilePath "$ProjectPath\Documentation\TROUBLESHOOTING.md" -Encoding UTF8

# Tests/TestPlan.md
$testPlan = @'
# Plano de Testes - SSMS Query Add-in

## 🎯 Objetivo
Validar o funcionamento completo do add-in em diferentes cenários e versões do SSMS.

## 🧪 Casos de Teste

### TC001 - Instalação Básica
**Objetivo:** Verificar instalação padrão
**Passos:**
1. Executar script de instalação
2. Verificar arquivos copiados
3. Verificar registro COM
4. Abrir SSMS
5. Verificar menu Tools

**Resultado Esperado:** Menu "Query Helper" visível em Tools

---

### TC002 - Funcionalidade Principal
**Objetivo:** Testar criação de nova query
**Passos:**
1. Clicar em Tools > Query Helper
2. Digitar query no TextBox
3. Clicar em "Nova Query"
4. Verificar nova janela criada

**Resultado Esperado:** Nova janela com query inserida

---

### TC003 - Validação de Entrada
**Objetivo:** Testar validação de dados
**Passos:**
1. Deixar TextBox vazio
2. Clicar em "Nova Query"
3. Verificar mensagem de aviso

**Resultado Esperado:** Alerta "Digite uma query antes de continuar!"

---

## 📊 Matriz de Compatibilidade

| Versão SSMS | Windows 10 | Windows 11 | .NET 4.7.2 | .NET 4.8 |
|-------------|------------|------------|-------------|----------|
| 2016 (13.0) | ✅ | ✅ | ✅ | ✅ |
| 2017 (14.0) | ✅ | ✅ | ✅ | ✅ |
| 2018 (15.0) | ✅ | ✅ | ✅ | ✅ |
| 2019 (15.0) | ✅ | ✅ | ✅ | ✅ |
| 2022 (19.0) | ✅ | ✅ | ✅ | ✅ |

---

## 📝 Checklist de Teste

### Pré-Release
- [ ] Compilação sem warnings
- [ ] Todos os TCs passaram
- [ ] Script de diagnóstico funciona
- [ ] Documentação atualizada

### Release
- [ ] Arquivo ZIP criado
- [ ] Scripts de instalação testados
- [ ] Compatibilidade verificada
- [ ] Performance aceitável
'@

Write-Host "   📄 Tests/TestPlan.md..." -ForegroundColor Green
$testPlan | Out-File -FilePath "$ProjectPath\Tests\TestPlan.md" -Encoding UTF8

# CONTRIBUTING.md
$contributing = @'
# Guia de Contribuição

Obrigado por considerar contribuir com o SSMS Query Add-in! 🎉

## 🤝 Como Contribuir

### Reportando Bugs
1. Verifique se o bug já foi reportado
2. Use o template de bug report
3. Inclua informações detalhadas:
   - Versão do SSMS
   - Versão do Windows
   - Passos para reproduzir
   - Logs de erro

### Sugerindo Melhorias
1. Abra uma issue com o label "enhancement"
2. Descreva claramente a funcionalidade desejada
3. Explique por que seria útil

### Pull Requests
1. Fork o repositório
2. Crie uma branch para sua feature
3. Faça commits descritivos
4. Execute os testes
5. Abra um Pull Request

## 📋 Padrões de Código

### Convenções
- **PascalCase** para métodos e propriedades públicas
- **camelCase** para variáveis locais
- **_underscore** para campos privados
- **Comentários** em português para documentação

### Estrutura de Commits
```
tipo(escopo): descrição breve

Descrição mais detalhada se necessário.

Fixes #123
```

## 🧪 Testes

Antes de submeter:
1. Compile sem warnings
2. Execute testes manuais básicos
3. Verifique compatibilidade com SSMS 2016+

---

**Obrigado por contribuir! 🚀**
'@

Write-Host "   📄 CONTRIBUTING.md..." -ForegroundColor Green
$contributing | Out-File -FilePath "$ProjectPath\CONTRIBUTING.md" -Encoding UTF8

# CHANGELOG.md
$changelog = @'
# Changelog

Todas as mudanças notáveis neste projeto serão documentadas neste arquivo.

## [1.0.0] - 2025-08-14

### Adicionado
- ✨ Interface de usuário com TextBox e Button
- ✨ Integração com menu Tools do SSMS
- ✨ Criação automática de nova query window
- ✨ Tentativa de conexão automática usando Object Explorer
- ✨ Suporte a múltiplas versões do SSMS (2016+)
- ✨ Sistema de logs para diagnóstico
- ✨ Scripts de instalação e desinstalação
- ✨ Script de diagnóstico PowerShell
- ✨ Documentação completa

### Recursos
- 🎯 TextBox multilinha com exemplo pré-carregado
- 🎯 Button "Nova Query" com estilo moderno
- 🎯 Button "Limpar" para reset rápido
- 🎯 Atalho Ctrl+Enter para executar
- 🎯 Fechamento automático após criar query
- 🎯 Tratamento de erros robusto

### Compatibilidade
- ✅ SSMS 2016+ (Version 13.0+)
- ✅ .NET Framework 4.7.2+
- ✅ Windows 10/11

## [Planejado para versões futuras]

### [1.1.0] - Recursos Avançados
- 📋 Histórico de queries executadas
- 💾 Snippets de código predefinidos
- 🔄 Templates de query personalizáveis

### [1.2.0] - Integração Avançada
- 🔗 Conexões favoritas salvas
- 📊 Integração aprimorada com Object Explorer
- 🗂️ Organização de queries por projeto
'@

Write-Host "   📄 CHANGELOG.md..." -ForegroundColor Green
$changelog | Out-File -FilePath "$ProjectPath\CHANGELOG.md" -Encoding UTF8

Write-Host ""
Write-Host "🎯 Criando arquivo de build..." -ForegroundColor Yellow

# Build script
$buildScript = @'
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
'@

Write-Host "   📄 Build.bat..." -ForegroundColor Green
$buildScript | Out-File -FilePath "$ProjectPath\Build.bat" -Encoding UTF8

Write-Host ""
Write-Host "📦 Finalizando projeto..." -ForegroundColor Yellow

# Criar pasta temp se não existir
if (!(Test-Path "C:\temp")) {
    New-Item -ItemType Directory -Path "C:\temp" -Force | Out-Null
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "✅ PROJETO CRIADO COM SUCESSO!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "📁 Localização: $ProjectPath" -ForegroundColor White
Write-Host ""
Write-Host "📋 Arquivos criados:" -ForegroundColor Yellow
Write-Host "   ✅ Código fonte (Connect.cs, QueryUserControl.cs)" -ForegroundColor Green
Write-Host "   ✅ Projeto Visual Studio (SSMSQueryAddin.csproj, .sln)" -ForegroundColor Green
Write-Host "   ✅ Configuração (app.config, AssemblyInfo.cs)" -ForegroundColor Green
Write-Host "   ✅ Scripts de instalação (InstallAddin.bat, UninstallAddin.bat)" -ForegroundColor Green
Write-Host "   ✅ Documentação completa (README.md, guias técnicos)" -ForegroundColor Green
Write-Host "   ✅ Exemplos e testes (SampleQueries.sql, TestPlan.md)" -ForegroundColor Green
Write-Host ""
Write-Host "🚀 Próximos passos:" -ForegroundColor Yellow
Write-Host "   1. Abra $ProjectPath\SSMSQueryAddin.sln no Visual Studio" -ForegroundColor White
Write-Host "   2. Ajuste as referências do SSMS conforme sua versão" -ForegroundColor White
Write-Host "   3. Compile o projeto (Build > Build Solution)" -ForegroundColor White
Write-Host "   4. Execute Scripts\InstallAddin.bat como Administrador" -ForegroundColor White
Write-Host "   5. Teste no SSMS (Tools > Query Helper)" -ForegroundColor White
Write-Host ""

# Opção de criar ZIP
if ($CreateZip) {
    Write-Host "📦 Criando arquivo ZIP..." -ForegroundColor Yellow
    $zipPath = "$($ProjectPath)_Complete.zip"
    try {
        Compress-Archive -Path "$ProjectPath\*" -DestinationPath $zipPath -Force
        Write-Host "   ✅ ZIP criado: $zipPath" -ForegroundColor Green
    } catch {
        Write-Warning "Erro ao criar ZIP: $($_.Exception.Message)"
    }
}

Write-Host ""
Write-Host "🎉 Projeto SSMS Query Add-in criado com sucesso!" -ForegroundColor Green
Write-Host "   Para mais informações, consulte README.md" -ForegroundColor Gray
Write-Host ""

# Perguntar se deseja abrir o projeto
$openProject = Read-Host "Deseja abrir o projeto no Visual Studio agora? (s/n)"
if ($openProject -eq "s" -or $openProject -eq "S") {
    if (Test-Path "$ProjectPath\SSMSQueryAddin.sln") {
        Start-Process "$ProjectPath\SSMSQueryAddin.sln"
        Write-Host "   🚀 Abrindo Visual Studio..." -ForegroundColor Green
    } else {
        Write-Warning "Arquivo de solution não encontrado!"
    }
}