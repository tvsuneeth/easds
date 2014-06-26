USE [CatererAndHotelKeeper_Systest]
GO
/****** Object:  StoredProcedure [chk].[GetArticlesModifiedSince]    Script Date: 06/26/2014 12:39:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [chk].[GetArticlesModifiedSince]
(		
	@modifiedDate DATETIME
)
AS
BEGIN

	SELECT a.liArticleId,a.dtLastModified FROM [dbo].Articles a
	WHERE a.dtApproved IS NOT NULL	AND a.dtLastModified > @modifiedDate		
	ORDER BY a.dtLastModified DESC
END



