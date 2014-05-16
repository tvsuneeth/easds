USE [CatererAndHotelKeeper_dev]
GO

/****** Object:  StoredProcedure [chk].[GetAsset]    Script Date: 16/05/2014 17:25:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [chk].[GetAsset]
(
	@AssetIds AS [chk].[ElementTable] READONLY
)
AS
BEGIN
	SELECT liAssetID, sAssetName, blobAsset, sFileExt
	FROM [dbo].[Assets]
	WHERE liAssetID IN (SELECT CAST(Name AS INT) AS Id FROM @AssetIds)
	AND (bDeleted IS NULL OR bDeleted = 0)
END
GO


