USE [DBNameHere]
GO
/****** Object:  StoredProcedure [easds].[GetArticle]    Script Date: 09/19/2014 12:17:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [easds].[GetArticle]
(
	@ArticleId int
)
AS
BEGIN



	SELECT
		  a.liArticleID
		, a.sHeadline
		, a.sAbbreviatedHeadline
		, a.sArticleSubHeadline
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
		, a.liNavigationItemID
		, s.liAssetID
		, s.sAssetName
		, s.sFileExt 
		, s.iHeight
		, s.iWidth
		, s.ImageCreatedDate
		, s.imageLastModifiedDate
	FROM
	(
		SELECT
			  a.liArticleID
			, a.sHeadline
			, a.sAbbreviatedHeadline
			, alp.sArticleSubHeadline
			, a.sIntro
			, a.sBody
			, a.dtPublicationDate
			, a.dtLastModified
			, a.dtExpiryDate
			, '' AS metaDescription
			, '' AS metaKeywords
			, a.liThumbnailID
			, a.liNavigationItemID
			, au.sTitle
			, au.sFirstName
			, au.sLastName
			, au.sEmailAddress
		FROM [dbo].[Articles] a
		LEFT OUTER JOIN [dbo].[Authors] au ON a.liAuthorID = au.liAuthorID
		LEFT OUTER JOIN [dbo].[ArticleLandingPage] alp ON a.liArticleID = alp.liArticleID
		WHERE a.liArticleStatusID=30 AND (a.dtReleaseDate IS NULL OR a.dtReleaseDate <= getdate()) AND ( a.dtExpiryDate IS NULL OR a.dtExpiryDate > getdate() )
		AND a.liArticleID =@ArticleId
	) AS a
	LEFT OUTER JOIN 
	(
		SELECT liAssetID, sAssetName, sFileExt, iHeight,iWidth, dtEntered as 'ImageCreatedDate', dtLastModified as 'imageLastModifiedDate'
		FROM [dbo].[Assets]
		WHERE (bDeleted IS NULL OR bDeleted = 0)
	) AS s ON a.liThumbnailID = s.liAssetID
END

