USE [CatererAndHotelKeeper_dev_latest]
GO
/****** Object:  StoredProcedure [chk].[GetStaticPage]    Script Date: 06/05/2014 14:04:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [chk].[GetListOfStaticPages]

AS
BEGIN
	SELECT 
		liStaticPageID,
		sPageURL

	FROM dbo.StaticPages
	WHERE bLive = 1
	ORDER BY liStaticPageID
END


