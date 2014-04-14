IF EXISTS
(
    SELECT * FROM sys.objects
    WHERE type = 'P' AND SCHEMA_NAME(schema_id) = 'chk' AND name like 'GetArticlesWithTaxonomy'
)
BEGIN
    DROP PROCEDURE [chk].[GetArticlesWithTaxonomy]
END

GO

CREATE PROCEDURE [chk].[GetArticlesWithTaxonomy]
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
	-- Included taxonomy element (fetch Article Section, Sector and Topic)
	-- We fetch for Article Section and Sector the given element and their child elements
	DECLARE @IncludedArticleSectionItems TABLE (Id INT NOT NULL)
	DECLARE @IncludedSectorItems TABLE (Id INT NOT NULL)
	DECLARE @IncludedTopicItems TABLE (Id INT NOT NULL);

	WITH articleSection (Id)
	AS
	(
		SELECT liCategoryItemID FROM [dbo].[CategoryItems] WHERE liCategoryID = 1 AND sItemName IN (SELECT Name FROM @IncludeArticleSectionNames)
		UNION ALL
		SELECT [dbo].[CategoryItems].liCategoryItemID
		FROM [dbo].[CategoryItems]
		INNER JOIN articleSection ON [dbo].[CategoryItems].liParentID = articleSection.Id
	)
	INSERT INTO @IncludedArticleSectionItems
	SELECT Id FROM articleSection;
	
	WITH sector (Id)
	AS
	(
		SELECT liCategoryItemID FROM [dbo].[CategoryItems] WHERE liCategoryID = 3 AND sItemName IN (SELECT Name FROM @IncludeSectorNames)
		UNION ALL
		SELECT [dbo].[CategoryItems].liCategoryItemID
		FROM [dbo].[CategoryItems]
		INNER JOIN sector ON [dbo].[CategoryItems].liParentID = sector.Id
	)
	INSERT INTO @IncludedSectorItems
	SELECT Id FROM sector;

	INSERT INTO @IncludedTopicItems
	SELECT liCategoryItemID FROM [dbo].[CategoryItems] WHERE liCategoryID = 2 AND sItemName IN (SELECT Name FROM @IncludeTopicNames)



	DECLARE @IncludedArticleSectionItemsCount INT
	DECLARE @IncludedSectorItemsCount INT
	DECLARE @IncludedTopicItemsCount INT
	SET @IncludedArticleSectionItemsCount = (SELECT count(*) FROM @IncludedArticleSectionItems)
	SET @IncludedSectorItemsCount = (SELECT count(*) FROM @IncludedSectorItems)
	SET @IncludedTopicItemsCount = (SELECT count(*) FROM @IncludedTopicItems)

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

	DECLARE @ArticleTable TABLE (ArticleId INT NOT NULL)
	INSERT INTO @ArticleTable
		SELECT DISTINCT liArticleID
		FROM
		(
			SELECT
				  amcis.liArticleID
				, CASE WHEN(@IncludedArticleSectionItemsCount > 0) THEN ISNULL(iasi.Id, 0) ELSE -1 END AS [IASI]
				, CASE WHEN(@IncludedSectorItemsCount > 0) THEN ISNULL(isi.Id, 0) ELSE -1 END AS [ISI]
				, CASE WHEN(@IncludedTopicItemsCount > 0) THEN ISNULL(iti.Id, 0) ELSE -1 END AS [ITI]
			FROM [dbo].[ArticleMultipleCategoryItemAssignments] amcis 
				LEFT OUTER JOIN @IncludedArticleSectionItems iasi ON amcis.liCategoryItemID = iasi.Id
				LEFT OUTER JOIN @IncludedSectorItems isi ON amcis.liCategoryItemID = isi.Id
				LEFT OUTER JOIN @IncludedTopicItems iti ON amcis.liCategoryItemID = iti.Id
			WHERE (iasi.Id IS NOT NULL OR isi.Id IS NOT NULL OR iti.Id IS NOT NULL)
		) AS alist
		GROUP BY liArticleID
		HAVING (SUM(ABS([IASI])) > 0 AND SUM(ABS([ISI])) > 0 AND SUM(ABS([ITI])) > 0)

	DELETE FROM @ArticleTable WHERE ArticleId IN
		(
			SELECT DISTINCT a.ArticleId
			FROM @ArticleTable AS a 
				INNER JOIN [dbo].[ArticleMultipleCategoryItemAssignments] c ON a.ArticleId = c.liArticleID
			WHERE c.liCategoryItemID IN (SELECT Id FROM @ExcludedCategoryItems)

		)


	-- Fetch article data based on the @ArticleTable 
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
			FROM @ArticleTable at LEFT OUTER JOIN [dbo].[Articles] a ON at.ArticleId = a.liArticleID
			WHERE a.dtApproved IS NOT NULL
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