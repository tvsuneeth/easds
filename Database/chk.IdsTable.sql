IF EXISTS
(
    SELECT * FROM sys.types
	WHERE is_user_defined = 1 AND SCHEMA_NAME(schema_id) = 'chk' AND name like 'IdsTable'
)
BEGIN
    DROP TYPE [chk].[IdsTable]
END

GO

CREATE TYPE [chk].[IdsTable] AS TABLE ( Id INT NOT NULL )