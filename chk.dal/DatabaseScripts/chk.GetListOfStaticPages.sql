USE [CatererAndHotelKeeper_systest]
GO
/****** Object:  StoredProcedure [chk].[GetListOfStaticPages]    Script Date: 06/25/2014 09:39:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [chk].[GetListOfStaticPages]

AS
BEGIN
	

select sp.liStaticPageID , pub.sPageURL  from  StaticPages sp
inner join StaticPages pub on sp.liLiveStaticPageID  = pub.liStaticPageID
where sp.liLiveStaticPageID is not null and pub.bLive=1 and pub.liLiveStaticPageID is null
order by sp.liStaticPageID

END


