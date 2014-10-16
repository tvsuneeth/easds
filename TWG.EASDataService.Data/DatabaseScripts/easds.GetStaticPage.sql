USE [DBNameHere]
GO
/****** Object:  StoredProcedure [easds].[GetStaticPage]    Script Date: 09/19/2014 12:22:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [easds].[GetStaticPage]
(
	@StaticPageId INT = -1,
	@StaticPageName NVARCHAR(255) = NULL
)
AS
BEGIN
    
 --   DECLARE @LiveStaticPageId INT
    
 --   Select @LiveStaticPageId 

	--SELECT TOP 1
	--	  liStaticPageID
	--	, sTitle
	--	, sBody
	--	, dtLastModified
	--	, sMetaDescription
	--	, sMetaKeywords
	--	, sPageURL
	--	, sPageTitle
	--FROM dbo.StaticPages
	--WHERE (@StaticPageId = -1 OR liStaticPageID = @StaticPageId)
	--AND (@StaticPageName IS NULL OR sPageURL LIKE @StaticPageName)
	--AND bLive = 1
	--ORDER BY dtLastModified DESC
	
	select sp.liStaticPageID 
	, pub.sTitle
	, pub.sBody
	, pub.dtLastModified
	, pub.sMetaDescription
	, pub.sMetaKeywords
	, pub.sPageURL
	, pub.sPageTitle  from  StaticPages sp
	inner join StaticPages pub on sp.liLiveStaticPageID  = pub.liStaticPageID
	where 
	(@StaticPageId = -1 OR sp.liStaticPageID = @StaticPageId)

END
