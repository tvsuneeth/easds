USE [CatererAndHotelKeeper_systest]
GO
/****** Object:  StoredProcedure [chk].[GetArticlesModifiedSince]    Script Date: 06/03/2014 10:39:16 ******/
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
