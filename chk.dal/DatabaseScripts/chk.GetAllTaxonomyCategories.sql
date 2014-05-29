USE [CatererAndHotelKeeper_systest]
GO
/****** Object:  StoredProcedure [chk].[GetAllTaxonomyCategories]    Script Date: 05/29/2014 11:43:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [chk].[GetAllTaxonomyCategories]
AS
BEGIN
	
	select c.liCategoryID,c.sCategoryName,ci.liCategoryItemID,ci.sItemName, ci.liParentID from 
	Categories c  left join categoryitems ci on c.liCategoryID = ci.liCategoryID
	where (ci.bRemoved IS NULL OR ci.bRemoved = 0) 
	order by c.liCategoryID,ci.liCategoryItemID
	
	--select * from dbo.Categories order by liCategoryID
	
	
END


