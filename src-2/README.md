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
