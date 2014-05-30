USE [CatererAndHotelKeeper_systest]
GO
/****** Object:  StoredProcedure [chk].[GetArticleTaxonomies]    Script Date: 05/30/2014 11:52:35 ******/
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

	Select c.liCategoryID, c.sCategoryName from Categories c where c.liCategoryID in
	(
		select ci.liCategoryID from ArticleMultipleCategoryItemAssignments  amc 
		inner join CategoryItems ci on ci.liCategoryItemID = amc.liCategoryItemID	    
		where amc.liArticleID = @ArticleId AND (ci.bRemoved IS NULL OR ci.bRemoved = 0)
	)
	
	select ci.liCategoryID, ci.liCategoryItemID,ci.sItemName, ci.liParentID from ArticleMultipleCategoryItemAssignments  amc 
	inner join CategoryItems ci on ci.liCategoryItemID = amc.liCategoryItemID	
	where amc.liArticleID = @ArticleId AND (ci.bRemoved IS NULL OR ci.bRemoved = 0)
	
	
	/*select ci.liCategoryItemID,ci.sItemName, c.liCategoryID,c.sCategoryName, ci.liParentID from ArticleMultipleCategoryItemAssignments  amc 
	inner join CategoryItems ci on ci.liCategoryItemID = amc.liCategoryItemID
	inner join Categories c on c.liCategoryID = ci.liCategoryID
	where amc.liArticleID = @ArticleId AND (ci.bRemoved IS NULL OR ci.bRemoved = 0)*/
	
END
