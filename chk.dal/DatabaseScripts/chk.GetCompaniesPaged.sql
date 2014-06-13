USE [CatererAndHotelKeeper_systest]
GO
/****** Object:  StoredProcedure [chk].[GetCompaniesPaged]    Script Date: 06/13/2014 14:56:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Suneeth Thikkal
-- Create date: March 2013
-- Description:	Gets the paged results of companies satrting with a character
-- =============================================
ALTER PROCEDURE [chk].[GetCompaniesPaged]
(
	@StartsWith nchar(1) = '',
	@SearchPhrase varchar(100) = '',
	@CategoryFilter varchar(1000) = ''
	--@CurrentPage int = 1,
	--@PageSize int = 10
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF (@StartsWith = '')
	BEGIN
		SELECT  
		 C.liCompanyID,  
		 C.sName,  
		 C.sDescription,  
		 C.sAddress1,  
		 C.sAddress2, 
		 cn.sCountry, 
		 C.sTown,  
		 C.sCounty,  
		 C.sCountryCode,  
		 C.sPostcode,  
		 C.sTelephone,  
		 C.sEmail,  
		 C.sFax,  
		 C.bRelativeURL,  
		 C.sURLTarget,  
		 C.sURLTitle,  
		 C.sURL,  
		 C.dAdSpend,  
		 C.liLogoID,  
		 C.sNotes,  
		 C.liCompanyStatusID,  
		 CSt.sCompanyStatus,  
		 C.uiLastModifiedByID,  
		 C.dtLastModified,  
		 U1.sWelcomeName as sLastModifiedBy,  
		 C.uiPublishedByID,  
		 C.dtPublished,  
		 U2.sWelcomeName as sPublishedBy,  
		 Cxtra.sActivities,  
		 Cxtra.sTimeLine,  
		 Cxtra.sFinancialSnapshot,  
		 Cxtra.sOperatingData,  
		 Cxtra.sStrategy,  
		 Cxtra.sKeyPeople,  
		 Cxtra.sCommentary,  
		 Cxtra.sChiefExecutive,  
		 -- Get SEO Information  
		 CLP.sMetaDescription,  
		 CLP.sMetaKeywords,  
		 CLP.sPageTitle,  
		 CLP.sLinks,  
		 CLP.sBlogLinks,  
		 CLP.sWebLinks,  
		 CLP.sCompanySearchTerm,   
		CLP.sCompanySubHeadline,  
		CLP.sCompanyEndURL,  
		CLP.bIsLandingPage,  
		CLP.sCustomHeaderElement,
		asset.liAssetID,
        asset.sAssetName,
        asset.iHeight,
        asset.iWidth,
        asset.sFileExt,
        asset.dtEntered as 'imageCreatedDate',
        asset.dtLastModified as 'imageLastModifiedDate'	  
		FROM           
		Companies C WITH (NOLOCK) 
		Inner Join Countries cn on C.sCountryCode = cn.sCountryCode  
		INNER JOIN CompanyStatuses CSt  WITH (NOLOCK) ON C.liCompanyStatusID = CSt.liCompanyStatusID  
		INNER JOIN Users U1  WITH (NOLOCK) on C.uiLastModifiedByID = U1.uiUserID  
		INNER JOIN Users U2  WITH (NOLOCK) on C.uiPublishedByID = U2.uiUserID  
		LEFT OUTER JOIN Caterer_CompanyExtras Cxtra WITH (NOLOCK) ON C.liCompanyID = Cxtra.liCompanyID  
		LEFT JOIN CompanyLandingPage CLP ON C.liCompanyID = CLP.liCompanyID
		LEFT JOIN dbo.Assets asset on C.liLogoID = asset.liAssetID		  
		WHERE CSt.liCompanyStatusID=20
		Order BY C.sName
	END
	
	IF (@StartsWith <> '')
	BEGIN
		select * 
		from    
		(
			SELECT	c.[liCompanyID], c.[sName], c.[sDescription],
					ROW_NUMBER() OVER (order by c.[sName] ) AS ROWID,
					COUNT(*) OVER () AS total_count  
			FROM	dbo.companies c
			WHERE	c.[liCompanyStatusID] = 20
			AND		c.[sName] LIKE (@StartsWith + '%')
		) as result      
		--where rowid > ((@CurrentPage - 1) * @PageSize) and rowid <(@CurrentPage * @PageSize + 1)
	END
	ELSE
	BEGIN
		DECLARE @categoriesFilterTab TABLE(Id int)
		INSERT @categoriesFilterTab SELECT Id FROM [dbo].ParamParserFn(@CategoryFilter, ',')
		
		-- perform article selection in searched categories
		DECLARE @articleInCategories TABLE(Id int)
		INSERT @articleInCategories
			SELECT DISTINCT CMCA.liCompanyID
			FROM CompanyMultipleCategoryItemAssignments CMCA
		WHERE CMCA.liCategoryItemID IN (SELECT * FROM @categoriesFilterTab)
			
		IF (@SearchPhrase <> '' AND @CategoryFilter <> '' )
		BEGIN
			SELECT * 
			FROM    
			(
				SELECT	c.[liCompanyID], c.[sName], c.[sDescription],
						ROW_NUMBER() OVER (order by s.[RANK] DESC) AS ROWID,
						COUNT(*) OVER () AS total_count  
				FROM	
					[dbo].[Companies] c
					INNER JOIN FREETEXTTABLE([dbo].[Companies], ([sName], [sDescription]), @SearchPhrase) s ON c.liCompanyID = s.[KEY]
				WHERE	c.[liCompanyStatusID] = 20
				AND c.[liCompanyID] IN (SELECT Id FROM @articleInCategories)
			) AS result      
			--WHERE rowid > ((@CurrentPage - 1) * @PageSize) and rowid <(@CurrentPage * @PageSize + 1)
		END
		ELSE
		BEGIN
			IF (@CategoryFilter <> '')
			BEGIN
				SELECT * 
				FROM    
				(
					SELECT	c.[liCompanyID], c.[sName], c.[sDescription],
							ROW_NUMBER() OVER (order by c.[sName]) AS ROWID,
							COUNT(*) OVER () AS total_count  
					FROM [dbo].[Companies] c
					WHERE	c.[liCompanyStatusID] = 20
					AND c.[liCompanyID] IN (SELECT Id FROM @articleInCategories)
				) AS result      
				--WHERE rowid > ((@CurrentPage - 1) * @PageSize) and rowid <(@CurrentPage * @PageSize + 1)
			END
			ELSE
			BEGIN
				IF (@SearchPhrase <> '')
				BEGIN
					select * 
					from    
					(
						SELECT	c.[liCompanyID], c.[sName], c.[sDescription],
								ROW_NUMBER() OVER (order by s.[RANK] DESC) AS ROWID,
								COUNT(*) OVER () AS total_count  
						FROM	[dbo].[Companies] c 
							INNER JOIN 
							FREETEXTTABLE([dbo].[Companies], ([sName], [sDescription]), @SearchPhrase) s
							ON c.liCompanyID = s.[KEY]
						WHERE	c.[liCompanyStatusID] = 20
					) as result      
					--where rowid > ((@CurrentPage - 1) * @PageSize) and rowid <(@CurrentPage * @PageSize + 1)
				END
			END
		END
	END
END