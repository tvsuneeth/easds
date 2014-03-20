IF EXISTS
(
    SELECT * FROM sys.types
	WHERE is_user_defined = 1 AND SCHEMA_NAME(schema_id) = 'chk' AND name like 'ElementTable'
)
BEGIN
    DROP TYPE [chk].[ElementTable]
END

GO

CREATE TYPE [chk].[ElementTable] AS TABLE ( Name NVARCHAR(255) NOT NULL )