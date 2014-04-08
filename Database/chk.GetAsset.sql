IF EXISTS
(
    SELECT * FROM sys.objects
    WHERE type = 'P' AND SCHEMA_NAME(schema_id) = 'chk' AND name like 'GetAsset'
)
BEGIN
    DROP PROCEDURE [chk].[GetAsset]
END

GO

CREATE PROCEDURE [chk].[GetAsset]
(
	@AssetIds AS [chk].[ElementTable] READONLY
)
AS
BEGIN
	SELECT liAssetID, sAssetName, blobAsset, sFileExt
	FROM [dbo].[Assets]
	WHERE liAssetID IN (SELECT CAST(Id AS INT) FROM @AssetIds)
	AND (bDeleted IS NULL OR bDeleted = 0)
END