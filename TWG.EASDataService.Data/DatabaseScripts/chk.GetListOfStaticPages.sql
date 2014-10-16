USE [DBNameHere]
GO
/****** Object:  StoredProcedure [easds].[GetListOfStaticPages]    Script Date: 09/19/2014 12:21:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [easds].[GetListOfStaticPages]

AS
BEGIN
	

select sp.liStaticPageID , pub.sPageURL  from  StaticPages sp
inner join StaticPages pub on sp.liLiveStaticPageID  = pub.liStaticPageID
where sp.liLiveStaticPageID is not null and pub.bLive=1 and pub.liLiveStaticPageID is null
order by sp.liStaticPageID

END


