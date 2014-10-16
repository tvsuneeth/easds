USE [DBNameHere]
GO
/****** Object:  StoredProcedure [easds].[GetAsset]    Script Date: 09/19/2014 12:20:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [easds].[GetAsset]
(
	@AssetIds AS [easds].[ElementTable] READONLY
)
AS
BEGIN
	SELECT liAssetID, sAssetName,sAssetDescription, blobAsset, sFileExt, iHeight,iWidth,dtEntered, dtLastModified, AT.liAssetTypeID, AT.sAssetType
	FROM [dbo].[Assets] A
	INNER JOIN AssetTypes AT on A.liAssetTypeID = AT.liAssetTypeID
	WHERE liAssetID IN (SELECT CAST(Name AS INT) AS Id FROM @AssetIds)
	AND (bDeleted IS NULL OR bDeleted = 0)
END

