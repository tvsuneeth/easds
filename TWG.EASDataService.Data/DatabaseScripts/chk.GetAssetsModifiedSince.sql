USE [CatererAndHotelKeeper_Systest]
GO
/****** Object:  StoredProcedure [chk].[GetAssetsModifiedSince]    Script Date: 09/09/2014 15:59:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [chk].[GetAssetsModifiedSince]
(		
	@modifiedDate DATETIME
)
AS
BEGIN

	SELECT a.liAssetID as 'ID',
	a.dtLastModified as 'ModifiedDate',
	(CASE WHEN  a.bDeleted IS NULL then 'Live'   
	      WHEN  a.bDeleted=1 then 'Deleted' END) 
	AS 'CurrentStatus' FROM [dbo].Assets a
	
	WHERE  a.dtLastModified > @modifiedDate		
	ORDER BY a.dtLastModified DESC
END


