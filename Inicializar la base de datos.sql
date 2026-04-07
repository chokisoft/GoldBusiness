USE [GoldBusiness];
GO

-- 1. Obtener todas las FK y eliminarlas primero
DECLARE @sql NVARCHAR(MAX) = '';

SELECT @sql = @sql + 
    'ALTER TABLE ' + QUOTENAME(SCHEMA_NAME(fk.schema_id)) + '.' + QUOTENAME(OBJECT_NAME(fk.parent_object_id)) + 
    ' DROP CONSTRAINT ' + QUOTENAME(fk.name) + ';' + CHAR(13)
FROM sys.foreign_keys fk;

-- Ejecutar eliminación de todas las FK
EXEC sp_executesql @sql;
GO

-- 2. Eliminar todas las tablas (ahora sin restricciones)
DECLARE @sql2 NVARCHAR(MAX) = '';

SELECT @sql2 = @sql2 + 
    'DROP TABLE IF EXISTS ' + QUOTENAME(SCHEMA_NAME(schema_id)) + '.' + QUOTENAME(name) + ';' + CHAR(13)
FROM sys.tables;

EXEC sp_executesql @sql2;
GO

-- 3. Verificar que no quedó ninguna tabla
SELECT COUNT(*) AS TablasRestantes FROM sys.tables;