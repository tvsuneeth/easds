USE [CatererAndHotelKeeper_systest]
GO
/****** Object:  StoredProcedure [chk].[GetAllTaxonomyCategoriesandItems]    Script Date: 05/30/2014 11:50:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [chk].[GetAllTaxonomyCategoriesandItems]
AS
BEGIN
	
	--select c.liCategoryID,c.sCategoryName,ci.liCategoryItemID,ci.sItemName, ci.liParentID from 
	--Categories c  left join categoryitems ci on c.liCategoryID = ci.liCategoryID
	--where (ci.bRemoved IS NULL OR ci.bRemoved = 0) 
	--order by c.liCategoryID,ci.liCategoryItemID
	
	Select c.liCategoryID, c.sCategoryName from Categories c
	order by c.liCategoryID
	
	select ci.liCategoryID, ci.liCategoryItemID,ci.sItemName, ci.liParentID from 
	CategoryItems ci where  (ci.bRemoved IS NULL OR ci.bRemoved = 0)
	order by ci.liCategoryID
	
END


