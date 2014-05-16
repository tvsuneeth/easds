USE [CatererAndHotelKeeper_dev]
GO

/****** Object:  StoredProcedure [chk].[GetStaticPage]    Script Date: 16/05/2014 17:26:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [chk].[GetStaticPage]
(
	@StaticPageId INT = -1,
	@StaticPageName NVARCHAR(255) = NULL
)
AS
BEGIN
	SELECT TOP 1
		  liStaticPageID
		, sTitle
		, sBody
		, dtLastModified
		, sMetaDescription
		, sMetaKeywords
		, sPageURL
		, sPageTitle
	FROM dbo.StaticPages
	WHERE (@StaticPageId = -1 OR liStaticPageID = @StaticPageId)
	AND (@StaticPageName IS NULL OR sPageURL LIKE @StaticPageName)
	AND bLive = 1
	ORDER BY dtLastModified DESC
END
GO


