USE [DBNameHere]
GO
/****** Object:  StoredProcedure [easds].[GetArticleTaxonomies]    Script Date: 09/19/2014 12:19:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [easds].[GetArticleTaxonomies]
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
		UNION
		select ci.liCategoryID from ArticleSingleCategoryItemAssignments  asca 
		inner join CategoryItems ci on ci.liCategoryItemID = asca.liCategoryItemID    
		where asca.liArticleID = @ArticleId AND (ci.bRemoved IS NULL OR ci.bRemoved = 0)
	)
	
	select ci.liCategoryID, ci.liCategoryItemID,ci.sItemName, ci.liParentID from ArticleMultipleCategoryItemAssignments  amc 
	inner join CategoryItems ci on ci.liCategoryItemID = amc.liCategoryItemID	
	where amc.liArticleID = @ArticleId AND (ci.bRemoved IS NULL OR ci.bRemoved = 0)
	UNION
	select ci.liCategoryID, ci.liCategoryItemID,ci.sItemName, ci.liParentID from ArticleSingleCategoryItemAssignments  asca 
	inner join CategoryItems ci on ci.liCategoryItemID = asca.liCategoryItemID	
	where asca.liArticleID = @ArticleId AND (ci.bRemoved IS NULL OR ci.bRemoved = 0)
	
	
END


	

