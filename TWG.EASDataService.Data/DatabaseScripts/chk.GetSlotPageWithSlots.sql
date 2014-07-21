USE [CatererAndHotelKeeper_Systest]
GO
/****** Object:  StoredProcedure [chk].[GetSlotPageWithSlots]    Script Date: 06/26/2014 12:42:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [chk].[GetSlotPageWithSlots] 
	@slotPageID int

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    DECLARE @$prog varchar(50), 
	        @$errno int, 
	        @$errmsg varchar(4000), 
	        @$proc_section_nm varchar(50),
	        @$row_cnt INT,
	        @$error_db_name varchar(50)

    SELECT @$errno = NULL,  @$errmsg = NULL,  @$proc_section_nm = NULL,
	       @$prog = LEFT(object_name(@@procid),50), @$row_cnt = NULL,
	       @$error_db_name = db_name();

    --==========
    BEGIN TRY
    --==========
    
		select * from dbo.SlotPages where liSlotPageID=@slotPageID
    
        SELECT
            s.liSlotID,
	        s.liSlotPageID,
	        s.sSlotName,
	        s.bHasImage,
            s.liImageID,
	        s.sImageAltTag,
	        s.bHasHeadline,
	        s.sHeadline,
	        s.bHasContent,
	        s.sContent,
	        s.bHasLink,
	        s.sURL,
	        s.bRelativeURL,
	        s.sURLTarget,
	        s.sURLTitle,
            s.sAccessKey,
	        s.sNotes,
	        s.bActive,
	        s.liOrder,
	        s.dtLastModified,
	        asset.liAssetID,
	        asset.sAssetName,
	        asset.iHeight,
	        asset.iWidth,
	        asset.sFileExt,
	        asset.dtEntered as 'imageCreatedDate',
	        asset.dtLastModified as 'imageLastModifiedDate'		
        FROM         
	        Slots S WITH (NOLOCK) 
			INNER JOIN dbo.SlotPages sp ON sp.[liSlotPageID] = S.[liSlotPageID]
			LEFT JOIN dbo.Assets asset on S.liImageID = asset.liAssetID			
        WHERE
			sp.[liSlotPageID] = @slotPageID
		AND	S.bActive = 1
		

		
        /* Error Handling Routine */
        SELECT @$errno = @@ERROR
        IF @$errno <> 0 
        BEGIN
	        SELECT @$errmsg = 'Error number ' +
						        CONVERT ( varchar(6), @$errno ) +
						        ' in procedure ' + @$prog 
            --EXEC dbo.ERROR_LOG_2005 @ERROR_LOG_PROGRAM_NM  = @$prog,  
	           --     @ERROR_LOG_PROGRAM_SECTION_NM          = @$proc_section_nm,
	           --     @ERROR_LOG_ERROR_NO     = @$errno, 
	           --     @ERROR_LOG_ERROR_DSC    = @$errmsg,
	           --     @ERROR_DB_NAME          = @$error_db_name
        END

    --===========
    END TRY
    --===========

    --===========
    BEGIN CATCH
    --===========
        SET @$errmsg = Left('Error ' +
		    CASE
			    WHEN @$errno > 0 THEN CAST(@$errno as varchar)
			    ELSE Cast(ERROR_NUMBER() as varchar)
		    END + 'in proc ' + isnull(@$prog,' ') + ' ' + 
		    CASE 
			    WHEN @$errno > 0 THEN isnull(@$errmsg,' ') 
			    ELSE isnull(@$errmsg,' ') + ISNULL(ERROR_MESSAGE(),'')
		    END ,4000);

        RAISERROR (@$errmsg, 16, 1); 

        --EXEC dbo.ERROR_LOG_2005 @ERROR_LOG_PROGRAM_NM  = @$prog,  
		      --  @ERROR_LOG_PROGRAM_SECTION_NM          = @$proc_section_nm,
		      --  @ERROR_LOG_ERROR_NO     = @$errno, 
		      --  @ERROR_LOG_ERROR_DSC    = @$errmsg,
		      --  @ERROR_DB_NAME          = @$error_db_name

        IF (ISNULL(@$errno,0) = 0 )
	        SET @$errno = ERROR_NUMBER();

    --===========
    END CATCH
    --===========

    SET NOCOUNT OFF; 

    RETURN @$errno;  

END -- End of stored procedures