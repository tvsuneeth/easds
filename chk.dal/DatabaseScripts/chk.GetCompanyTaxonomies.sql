﻿USE [CatererAndHotelKeeper_Systest]
GO
/****** Object:  StoredProcedure [chk].[GetCompanyTaxonomies]    Script Date: 06/26/2014 12:41:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [chk].[GetCompanyTaxonomies]
(
	@CompanyId INT
)
AS
BEGIN

	Select c.liCategoryID, c.sCategoryName from Categories c where c.liCategoryID in
	(
		select ci.liCategoryID from CompanyMultipleCategoryItemAssignments  cmc 
		inner join CategoryItems ci on ci.liCategoryItemID = cmc.liCategoryItemID	    
		where cmc.liCompanyID = @CompanyId AND (ci.bRemoved IS NULL OR ci.bRemoved = 0)
		UNION
		select ci.liCategoryID from CompanySingleCategoryItemAssignments  csca 
		inner join CategoryItems ci on ci.liCategoryItemID = csca.liCategoryItemID    
		where csca.liCompanyID = @CompanyId AND (ci.bRemoved IS NULL OR ci.bRemoved = 0)
	)
	
	select ci.liCategoryID, ci.liCategoryItemID,ci.sItemName, ci.liParentID from CompanyMultipleCategoryItemAssignments  cmc 
	inner join CategoryItems ci on ci.liCategoryItemID = cmc.liCategoryItemID	
	where cmc.liCompanyID = @CompanyId AND (ci.bRemoved IS NULL OR ci.bRemoved = 0)
	UNION
	select ci.liCategoryID, ci.liCategoryItemID,ci.sItemName, ci.liParentID from CompanySingleCategoryItemAssignments  csca 
	inner join CategoryItems ci on ci.liCategoryItemID = csca.liCategoryItemID	
	where csca.liCompanyID = @CompanyId AND (ci.bRemoved IS NULL OR ci.bRemoved = 0)
	
	
END
