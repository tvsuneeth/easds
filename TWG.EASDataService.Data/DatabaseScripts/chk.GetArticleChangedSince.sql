USE [CatererAndHotelKeeper_Systest]
GO
/****** Object:  StoredProcedure [chk].[GetArticlesChangedSince]    Script Date: 09/19/2014 12:18:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [chk].[GetArticlesChangedSince]
(		
	@changedDate DATETIME
)
AS
BEGIN
    
    DECLARE @currDate DATETIME
    --SET @currDate = '2014/07/25 01:13:00 AM'
    SET @currDate = GETDATE()
    
	SELECT  A.liArticleId as 'ID',
	A.dtLastModified as 'LastModified',	 
	A.dtReleaseDate as 'ReleaseDate',
	A.dtExpiryDate 'ExpiryDate' ,
	A.liArticleStatusID 'StatusID',
	(CASE 	
	WHEN (A.liArticleStatusID=30 AND A.dtReleaseDate>@currDate) THEN 'NotReleased'             
	WHEN (A.liArticleStatusID=30 AND  A.dtExpiryDate<=@currDate)  THEN 'Expired'
	WHEN (A.liArticleStatusID=30 AND  (A.dtExpiryDate>@currDate OR A.dtExpiryDate IS NULL)  AND  A.dtReleaseDate<=@currDate )  THEN 'Live'
	WHEN AST.sArticleStatus = 'Under Review' THEN 'UnderReview'
	ELSE AST.sArticleStatus
    END) AS 'CurrentStatus' 
             
	FROM [dbo].Articles A
	inner join ArticleStatuses AST on AST.liArticleStatusID = A.liArticleStatusID 
	WHERE (A.dtLastModified >= @changedDate)  -- modified since
	OR 
	( A.liArticleStatusID=30 AND A.dtExpiryDate >= @changedDate AND  A.dtExpiryDate <= @currDate ) --naturally expired since
	OR 
	( A.liArticleStatusID=30 AND A.dtReleaseDate >= @changedDate AND  A.dtReleaseDate <= @currDate ) --naturally released since
	
	ORDER BY a.dtLastModified DESC
		
	
	
END





