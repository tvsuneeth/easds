USE [CatererAndHotelKeeper_dev]
GO

/****** Object:  StoredProcedure [chk].[GetArticle]    Script Date: 16/05/2014 17:22:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [chk].[GetArticle]
(
	@ArticleIds AS [chk].[ElementTable] READONLY
)
AS
BEGIN

select * from @ArticleIds

	SELECT
		  a.liArticleID
		, a.sHeadline
		, a.sIntro
		, a.sBody
		, a.dtPublicationDate
		, a.dtLastModified
		, a.dtExpiryDate
		, a.metaDescription
		, a.metaKeywords
		, a.sTitle
		, a.sFirstName
		, a.sLastName
		, a.sEmailAddress
		, s.*
	FROM
	(
		SELECT
			  a.liArticleID
			, a.sHeadline
			, a.sIntro
			, a.sBody
			, a.dtPublicationDate
			, a.dtLastModified
			, a.dtExpiryDate
			, '' AS metaDescription
			, '' AS metaKeywords
			, a.liThumbnailID
			, au.sTitle
			, au.sFirstName
			, au.sLastName
			, au.sEmailAddress
		FROM [dbo].[Articles] a
		LEFT OUTER JOIN [dbo].[Authors] au ON a.liAuthorID = au.liAuthorID
		WHERE a.dtApproved IS NOT NULL
		AND a.liArticleID IN (SELECT CAST(Name AS INT) AS Id  FROM @ArticleIds)
	) AS a
	LEFT OUTER JOIN 
	(
		SELECT liAssetID, sAssetName, sFileExt
		FROM [dbo].[Assets]
		WHERE (bDeleted IS NULL OR bDeleted = 0)
	) AS s ON a.liThumbnailID = s.liAssetID
END
GO


