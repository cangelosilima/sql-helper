# FI.Developer.SqlServerHelper

Uma solução completa para geração de scripts SQL Server com suporte a CLI e Add-in para SQL Server Management Studio.

## Funcionalidades

### CLI Tool

- **Geração de Procedures**: Cria automaticamente procedures de Upsert e Delete para tabelas
- **Gerenciamento de Templates**: Permite criar e gerenciar templates customizados
- **Scripts de Execução**: Gera scripts de execução a partir de dados CSV

### SSMS Add-in

- **Integração com Object Explorer**: Menu de contexto nas tabelas
- **Integração com Editor**: Menu de contexto no editor SQL
- **Interface Visual**: Forms para configuração e preview
- **Geração em Lote**: Processar múltiplas tabelas simultaneamente

## Instalação

### CLI Tool

`ash
dotnet tool install --global fi-sql-helper
`

### SSMS Add-in

1. Compile o projeto FI.Developer.SqlServerHelper.SSMS
2. Os arquivos serão copiados automaticamente para a pasta de extensões do SSMS
3. Reinicie o SQL Server Management Studio

## Uso

### CLI

`ash
# Gerar procedures para todas as tabelas
fi-sql-helper generate --connection-string "..." --output ./scripts

# Gerar apenas para uma tabela específica
fi-sql-helper generate --connection-string "..." --schema dbo --table MyTable --output ./scripts

# Listar templates disponíveis
fi-sql-helper template list

# Criar novo template
fi-sql-helper template create --id my-template --name "My Template" --file template.sql

# Gerar script de execução
fi-sql-helper execute --connection-string "..." --procedure dbo.MyProc --csv data.csv --output exec.sql
`

### SSMS Add-in

1. **Object Explorer**: Clique com botão direito em uma tabela → "Generate SQL Scripts"
2. **Editor SQL**: Clique com botão direito → "Generate SQL Scripts"
3. **Result Grid**: Clique com botão direito → "Generate Execution Script"

## Templates

### Placeholders Disponíveis

- {{SchemaName}} - Nome do schema
- {{TableName}} - Nome da tabela
- {{DatabaseName}} - Nome do banco de dados
- {{Columns}} - Lista de colunas
- {{Parameters}} - Parâmetros da procedure
- {{PrimaryKeyColumns}} - Colunas da chave primária
- {{UpdateColumns}} - Colunas para UPDATE
- {{InsertColumns}} - Colunas para INSERT

### Exemplo de Template

`sql
CREATE OR ALTER PROCEDURE [{{SchemaName}}].[Sp_{{DatabaseName}}_{{TableName}}_Get]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT {{Columns}}
    FROM [{{SchemaName}}].[{{TableName}}]
    WHERE Id = @Id;
END
`

## Configuração

Os templates customizados são salvos em:
`
%APPDATA%\FI.Developer.SqlServerHelper\Templates
`

## Desenvolvimento

### Estrutura do Projeto

- FI.Developer.SqlServerHelper.Core - Lógica principal e serviços
- FI.Developer.SqlServerHelper.CLI - Ferramenta de linha de comando
- FI.Developer.SqlServerHelper.SSMS - Add-in para SSMS

### Requisitos

- .NET 8.0
- SQL Server Management Studio 19+
- Visual Studio 2022

### Compilação

`ash
dotnet build
`

### Testes

`ash
dotnet test
`

## Contribuindo

1. Fork o projeto
2. Crie uma branch para sua feature (git checkout -b feature/AmazingFeature)
3. Commit suas mudanças (git commit -m 'Add some AmazingFeature')
4. Push para a branch (git push origin feature/AmazingFeature)
5. Abra um Pull Request

## Licença

Este projeto está licenciado sob a MIT License - veja o arquivo LICENSE para detalhes.
