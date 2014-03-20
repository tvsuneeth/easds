IF EXISTS
(
    SELECT * FROM sys.objects
    WHERE type = 'P' AND SCHEMA_NAME(schema_id) = 'chk' AND name like 'GetArticles'
)
BEGIN
    DROP PROCEDURE [chk].[GetArticles]
END

GO

CREATE PROCEDURE [chk].[GetArticles]
(
	@IncludeArticleSectionNames AS [chk].[ElementTable] READONLY,
	@IncludeSectorNames AS [chk].[ElementTable] READONLY,
	@IncludeTopicNames AS [chk].[ElementTable] READONLY,
	@ExcludeArticleSectionNames AS [chk].[ElementTable] READONLY,
	@ExcludeSectorNames AS [chk].[ElementTable] READONLY,
	@ExcludeTopicNames AS [chk].[ElementTable] READONLY,
	@PageNumber INT,
	@PageSize INT
)
AS
BEGIN
	-- Included taxonomy element
	DECLARE @IncludedCategoryItems TABLE (Id INT NOT NULL)
	INSERT INTO @IncludedCategoryItems SELECT liCategoryItemID FROM [dbo].[CategoryItems] WHERE liCategoryID = 1 AND sItemName IN (SELECT Name FROM @IncludeArticleSectionNames)
	INSERT INTO @IncludedCategoryItems SELECT liCategoryItemID FROM [dbo].[CategoryItems] WHERE liCategoryID = 3 AND sItemName IN (SELECT Name FROM @IncludeSectorNames)
	INSERT INTO @IncludedCategoryItems SELECT liCategoryItemID FROM [dbo].[CategoryItems] WHERE liCategoryID = 2 AND sItemName IN (SELECT Name FROM @IncludeTopicNames)

	DECLARE @v XML = (SELECT Id FROM @IncludedCategoryItems FOR XML AUTO)
	DECLARE @v2 XML = (SELECT Name FROM @IncludeTopicNames FOR XML AUTO)

	-- Excluded taxonomy element
	DECLARE @ExcludedCategoryItems TABLE (Id INT NOT NULL)
	INSERT INTO @ExcludedCategoryItems SELECT liCategoryItemID FROM [dbo].[CategoryItems] WHERE liCategoryID = 1 AND sItemName IN (SELECT Name FROM @ExcludeArticleSectionNames)
	INSERT INTO @ExcludedCategoryItems SELECT liCategoryItemID FROM [dbo].[CategoryItems] WHERE liCategoryID = 3 AND sItemName IN (SELECT Name FROM @ExcludeSectorNames)
	INSERT INTO @ExcludedCategoryItems SELECT liCategoryItemID FROM [dbo].[CategoryItems] WHERE liCategoryID = 2 AND sItemName IN (SELECT Name FROM @ExcludeTopicNames)

	DECLARE @ArticleTable TABLE (ArticleId INT NOT NULL)
	INSERT INTO @ArticleTable	
		SELECT DISTINCT liArticleID FROM [dbo].[ArticleMultipleCategoryItemAssignments]
		WHERE liCategoryItemID NOT IN (SELECT Id FROM @ExcludedCategoryItems)
		AND liCategoryItemID IN (SELECT Id FROM @IncludedCategoryItems)

	SELECT
		  ta.liArticleID
		, ta.sHeadline
		, ta.sIntro
		, ta.sBody
		, ta.dtPublicationDate
		, ta.dtLastModified
		, ta.dtExpiryDate
		, ta.metaDescription
		, ta.metaKeywords
		, ta.TotalNumberOfRow
	FROM
	(
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
			, CAST(ROW_NUMBER() OVER (ORDER BY a.dtPublicationDate DESC) AS INT) AS RowNumber
			, COUNT(*) OVER () AS TotalNumberOfRow
		FROM @ArticleTable at LEFT OUTER JOIN [dbo].[Articles] a ON at.ArticleId = a.liArticleID
		WHERE a.dtApproved IS NOT NULL
	) AS ta
	WHERE ta.RowNumber BETWEEN (@PageNumber * @PageSize) + 1 AND (@PageNumber + 1) * @PageSize
END