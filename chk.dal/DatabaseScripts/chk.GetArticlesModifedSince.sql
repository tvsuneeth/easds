USE [CatererAndHotelKeeper_systest]
GO
/****** Object:  StoredProcedure [chk].[GetArticlesModifedSince]    Script Date: 05/29/2014 11:37:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





ALTER PROCEDURE [chk].[GetArticlesModifedSince]
(
	@ExcludeArticleSectionNames AS [chk].[ElementTable] READONLY,
	@ExcludeSectorNames AS [chk].[ElementTable] READONLY,
	@ExcludeTopicNames AS [chk].[ElementTable] READONLY,
	
	@modifiedDate DATETIME
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
			AND a.dtLastModified > @modifiedDate
		) AS ta
	--	WHERE  ta.RowNumber BETWEEN (@PageNumber * @PageSize) + 1 AND (@PageNumber + 1) * @PageSize
	) AS a LEFT OUTER JOIN [dbo].[Authors] AS au ON a.liAuthorID = au.liAuthorID
	LEFT OUTER JOIN 
	(
		SELECT liAssetID, sAssetName, sFileExt
		FROM [dbo].[Assets]
		WHERE (bDeleted IS NULL OR bDeleted = 0)
	) AS s ON a.liThumbnailID = s.liAssetID
	ORDER BY a.dtLastModified DESC
END



