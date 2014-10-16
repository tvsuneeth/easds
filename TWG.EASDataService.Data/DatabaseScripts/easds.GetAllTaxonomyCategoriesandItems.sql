USE [DBNameHere]
GO
/****** Object:  StoredProcedure [easds].[GetAllTaxonomyCategoriesandItems]    Script Date: 09/19/2014 12:17:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [easds].[GetAllTaxonomyCategoriesandItems]
AS
BEGIN
		
	
	Select c.liCategoryID, c.sCategoryName from Categories c
	order by c.liCategoryID
	
	select ci.liCategoryID, ci.liCategoryItemID,ci.sItemName, ci.liParentID from 
	CategoryItems ci where  (ci.bRemoved IS NULL OR ci.bRemoved = 0)
	order by ci.liCategoryID
		
	
END


