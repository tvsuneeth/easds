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
	@ExcludeArticleSectionNames AS [chk].[ElementTable] READONLY,
	@ExcludeSectorNames AS [chk].[ElementTable] READONLY,
	@ExcludeTopicNames AS [chk].[ElementTable] READONLY,
	@PageNumber INT,
	@PageSize INT
)
AS
BEGIN

	-- Excluded taxonomy element
	-- We fetch the given element and all child element of them
	DECLARE @ExcludedCategoryItems TABLE (Id INT NOT NULL);

	WITH excludedItem (Id)
	AS
	(
		SELECT liCategoryItemID
		FROM [dbo].[CategoryItems]
		WHERE
			(
				(liCategoryID = 1 AND sItemName IN (SELECT Name FROM @ExcludeArticleSectionNames))
				OR
				(liCategoryID = 3 AND sItemName IN (SELECT Name FROM @ExcludeSectorNames))
				OR
				(liCategoryID = 2 AND sItemName IN (SELECT Name FROM @ExcludeTopicNames))
			)
		UNION ALL
		SELECT [dbo].[CategoryItems].liCategoryItemID
		FROM [dbo].[CategoryItems]
		INNER JOIN excludedItem ON [dbo].[CategoryItems].liParentID = excludedItem.Id
	)
	INSERT INTO @ExcludedCategoryItems
	SELECT Id FROM excludedItem;

	-- excluded article id list
	DECLARE @ExcludedArticleTable TABLE (ArticleId INT NOT NULL)
	
	INSERT INTO @ExcludedArticleTable
	SELECT DISTINCT c.liArticleID
	FROM [dbo].[ArticleMultipleCategoryItemAssignments] c
	WHERE c.liCategoryItemID IN (SELECT Id FROM @ExcludedCategoryItems)


	-- Fetch articles 
	SELECT
		  a.liArticleID
		, a.sHeadline
		, a.sIntro
		, a.dtPublicationDate
		, a.dtLastModified
		, a.TotalNumberOfRow
		, au.sTitle
		, au.sFirstName
		, au.sLastName
		, au.sEmailAddress
		, s.*
	FROM
	(
		SELECT *
		FROM
		(
			SELECT
				*
				, '' AS metaDescription
				, '' AS metaKeywords
				, CAST(ROW_NUMBER() OVER (ORDER BY a.dtPublicationDate DESC) AS INT) AS RowNumber
				, COUNT(*) OVER () AS TotalNumberOfRow
			FROM [dbo].[Articles] a
			WHERE a.dtApproved IS NOT NULL
			AND a.liArticleID NOT IN (SELECT ArticleId FROM @ExcludedArticleTable)
		) AS ta
		WHERE ta.RowNumber BETWEEN (@PageNumber * @PageSize) + 1 AND (@PageNumber + 1) * @PageSize
	) AS a LEFT OUTER JOIN [dbo].[Authors] AS au ON a.liAuthorID = au.liAuthorID
	LEFT OUTER JOIN 
	(
		SELECT liAssetID, sAssetName, sFileExt
		FROM [dbo].[Assets]
		WHERE (bDeleted IS NULL OR bDeleted = 0)
	) AS s ON a.liThumbnailID = s.liAssetID
END