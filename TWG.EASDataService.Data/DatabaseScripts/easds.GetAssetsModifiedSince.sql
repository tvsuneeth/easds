USE [DBNameHere]
GO
/****** Object:  StoredProcedure [easds].[GetAssetsModifiedSince]    Script Date: 09/19/2014 12:20:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [easds].[GetAssetsModifiedSince]
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


