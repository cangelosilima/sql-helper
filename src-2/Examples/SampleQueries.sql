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
