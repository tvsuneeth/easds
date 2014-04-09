IF EXISTS
(
    SELECT * FROM sys.objects
    WHERE type = 'P' AND SCHEMA_NAME(schema_id) = 'chk' AND name like 'GetArticleTaxonomy'
)
BEGIN
    DROP PROCEDURE [chk].[GetArticleTaxonomy]
END

GO

CREATE PROCEDURE [chk].[GetArticleTaxonomy]
(
	@ArticleId INT
)
AS
BEGIN
	WITH taxonomyItems (liCategoryItemID, liCategoryID, sItemName, liParentID)
	AS
	(
		SELECT ci.liCategoryItemID, ci.liCategoryID, ci.sItemName, ci.liParentID
		FROM [dbo].[CategoryItems] ci INNER JOIN
		(
			SELECT DISTINCT amcia.liCategoryItemID
			FROM [dbo].[ArticleMultipleCategoryItemAssignments] amcia
			WHERE amcia.liArticleID = @ArticleId
			AND amcia.liCategoryID in (1, 2, 3)
		) as t ON ci.liCategoryItemID = t.liCategoryItemID
		WHERE (ci.bRemoved IS NULL OR ci.bRemoved = 0)
		UNION ALL
		SELECT ci2.liCategoryItemID, ci2.liCategoryID, ci2.sItemName, ci2.liParentID 
		FROM [dbo].[CategoryItems] ci2
		INNER JOIN taxonomyItems ON ci2.liCategoryItemID = taxonomyItems.liParentID
		WHERE (ci2.bRemoved IS NULL OR ci2.bRemoved = 0)
	)
	SELECT DISTINCT *
	FROM taxonomyItems
	ORDER BY liCategoryID, liParentID, liCategoryItemID, sItemName
END