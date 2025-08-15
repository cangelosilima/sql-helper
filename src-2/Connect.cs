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
