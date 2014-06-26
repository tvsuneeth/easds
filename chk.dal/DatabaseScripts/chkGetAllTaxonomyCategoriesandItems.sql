USE [CatererAndHotelKeeper_Systest]
GO
/****** Object:  StoredProcedure [chk].[GetAllTaxonomyCategoriesandItems]    Script Date: 06/26/2014 12:38:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [chk].[GetAllTaxonomyCategoriesandItems]
AS
BEGIN
		
	
	Select c.liCategoryID, c.sCategoryName from Categories c
	order by c.liCategoryID
	
	select ci.liCategoryID, ci.liCategoryItemID,ci.sItemName, ci.liParentID from 
	CategoryItems ci where  (ci.bRemoved IS NULL OR ci.bRemoved = 0)
	order by ci.liCategoryID
		
	
END


