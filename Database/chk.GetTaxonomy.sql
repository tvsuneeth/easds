IF EXISTS
(
    SELECT * FROM sys.objects
    WHERE type = 'P' AND SCHEMA_NAME(schema_id) = 'chk' AND name like 'GetTaxonomy'
)
BEGIN
    DROP PROCEDURE [chk].[GetTaxonomy]
END

GO

CREATE PROCEDURE [chk].[GetTaxonomy]
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
	);
		

	-- fetch taxonom list with parents
	WITH taxonomyItems (liCategoryItemID, liCategoryID, sItemName, liParentID)
	AS
	(
		SELECT ci.liCategoryItemID, ci.liCategoryID, ci.sItemName, ci.liParentID
		FROM [dbo].[CategoryItems] ci INNER JOIN @TaxonomyIdTable AS t ON ci.liCategoryItemID = t.Id
		UNION ALL
		SELECT ci2.liCategoryItemID, ci2.liCategoryID, ci2.sItemName, ci2.liParentID 
		FROM [dbo].[CategoryItems] ci2
		INNER JOIN taxonomyItems ON ci2.liCategoryItemID = taxonomyItems.liParentID
	)
	SELECT DISTINCT *
	FROM taxonomyItems
	WHERE liCategoryItemID NOT IN (SELECT Id FROM @TaxonomyIdTable)
	ORDER BY liCategoryID, liParentID, liCategoryItemID, sItemName
END