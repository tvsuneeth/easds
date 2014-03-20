IF EXISTS
(
    SELECT * FROM sys.objects
    WHERE type = 'P' AND SCHEMA_NAME(schema_id) = 'chk' AND name like 'GetStaticPage'
)
BEGIN
    DROP PROCEDURE [chk].[GetStaticPage]
END

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