USE [CatererAndHotelKeeper_systest]
GO
/****** Object:  StoredProcedure [chk].[GetArticleTaxonomies]    Script Date: 05/29/2014 11:38:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [chk].[GetArticleTaxonomies]
(
	@ArticleId INT
)
AS
BEGIN
	select ci.liCategoryItemID,ci.sItemName, c.liCategoryID,c.sCategoryName, ci.liParentID from ArticleMultipleCategoryItemAssignments  amc 
	inner join CategoryItems ci on ci.liCategoryItemID = amc.liCategoryItemID
	inner join Categories c on c.liCategoryID = ci.liCategoryID
	where amc.liArticleID = @ArticleId AND (ci.bRemoved IS NULL OR ci.bRemoved = 0)
END
