USE [CatererAndHotelKeeper_Systest]
GO
/****** Object:  StoredProcedure [chk].[GetAsset]    Script Date: 06/26/2014 12:41:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [chk].[GetAsset]
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

