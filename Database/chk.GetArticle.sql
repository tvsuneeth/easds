IF EXISTS
(
    SELECT * FROM sys.objects
    WHERE type = 'P' AND SCHEMA_NAME(schema_id) = 'chk' AND name like 'GetArticle'
)
BEGIN
    DROP PROCEDURE [chk].[GetArticle]
END

GO

CREATE PROCEDURE [chk].[GetArticle]
(
	@ArticleIds AS [chk].[ElementTable] READONLY
)
AS
BEGIN
	SELECT
		  a.liArticleID
		, a.sHeadline
		, a.sIntro
		, a.sBody
		, a.dtPublicationDate
		, a.dtLastModified
		, a.dtExpiryDate
		, '' AS metaDescription
		, '' AS metaKeywords
	FROM [dbo].[Articles] a
	WHERE a.dtApproved IS NOT NULL
	AND a.liArticleID IN (SELECT CAST(Name AS INT) AS Id  FROM @ArticleIds)
END