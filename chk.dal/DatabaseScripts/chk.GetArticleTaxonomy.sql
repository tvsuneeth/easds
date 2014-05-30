USE [CatererAndHotelKeeper_systest]
GO
/****** Object:  StoredProcedure [chk].[GetArticleTaxonomy]    Script Date: 05/30/2014 11:52:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [chk].[GetArticleTaxonomy]
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
