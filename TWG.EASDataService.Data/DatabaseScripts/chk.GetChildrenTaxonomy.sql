USE [DBNameHere]
GO
/****** Object:  StoredProcedure [easds].[GetChildrenTaxonomy]    Script Date: 09/19/2014 12:21:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [easds].[GetChildrenTaxonomy]
(
	@CategoryItemId INT = 0,
	@ArticleSectionName NVARCHAR(255) = NULL,
	@SectorName NVARCHAR(255) = NULL,
	@TopicName NVARCHAR(255) = NULL
)
AS
BEGIN
	-- fetch article sections, sectors and topics ids from CategoryItem table
	DECLARE @TaxonomyIdTable TABLE (Id INT NOT NULL);

	INSERT INTO @TaxonomyIdTable
	SELECT liCategoryItemID
	FROM [dbo].[CategoryItems]
	WHERE
	(
		(@CategoryItemId > 0 AND liCategoryItemID = @CategoryItemId)
		OR
		(@ArticleSectionName IS NOT NULL AND sItemName like @ArticleSectionName AND liCategoryID = 1)
		OR
		(@SectorName IS NOT NULL AND sItemName like @SectorName AND liCategoryID = 3)
		OR
		(@TopicName IS NOT NULL AND sItemName like @TopicName AND liCategoryID = 2)
	)
	AND (bRemoved IS NULL OR bRemoved = 0);
		

	-- fetch taxonomy list with direct children (only one level down)
	WITH taxonomyItems (liCategoryItemID, liCategoryID, sItemName, liParentID)
	AS
	(
		SELECT ci2.liCategoryItemID, ci2.liCategoryID, ci2.sItemName, ci2.liParentID
		FROM [dbo].[CategoryItems] ci INNER JOIN @TaxonomyIdTable AS t ON ci.liCategoryItemID = t.Id
		INNER JOIN [dbo].[CategoryItems] ci2 ON ci.liCategoryItemID = ci2.liParentID
		WHERE (ci2.bRemoved IS NULL OR ci2.bRemoved = 0)
	)
	SELECT DISTINCT *
	FROM taxonomyItems
	WHERE liCategoryItemID NOT IN (SELECT Id FROM @TaxonomyIdTable)
	ORDER BY liCategoryID, liParentID, liCategoryItemID, sItemName
END
