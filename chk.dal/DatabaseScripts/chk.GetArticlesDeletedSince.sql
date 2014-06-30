USE [CatererAndHotelKeeper_dev]
GO
/****** Object:  StoredProcedure [chk].[GetArticlesModifiedSince]    Script Date: 06/30/2014 15:46:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Alter PROCEDURE [chk].[GetArticlesDeletedSince]
(		
	@deletedDate DATETIME
)
AS
BEGIN

	SELECT a.liArticleId as 'Id',a.dtLastModified as 'DeletedDate' FROM [dbo].Articles a
	WHERE a.dtApproved IS NULL	
	AND a.liArticleStatusID=40 
	AND a.dtLastModified > @deletedDate		
	ORDER BY a.dtLastModified DESC
END



