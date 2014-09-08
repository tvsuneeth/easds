﻿USE [CatererAndHotelKeeper_Systest]
GO
/****** Object:  StoredProcedure [chk].[GetListOfStaticPages]    Script Date: 06/26/2014 12:42:00 ******/
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

