IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateAppVersion]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateAppVersion]
GO

CREATE PROCEDURE [dbo].[sp_CreateAppVersion](
    @PlatformKey UNIQUEIDENTIFIER,
	@LatestBuild INT,
	@LatestVersion [NVARCHAR](64),
	@MinRequiredBuild INT,
    @Note [NVARCHAR](MAX),
    @AppServiceStatus INT,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
BEGIN    
    DECLARE @NowTime AS DATETIME = GETUTCDATE();
    DECLARE @Key AS UNIQUEIDENTIFIER = NEWID();
    DECLARE @ErrerMessage AS NVARCHAR(MAX);
    DECLARE @ErrorSeverity AS INT;
    DECLARE @ErrorState AS INT;
    DECLARE @ErrorCode AS INT;

    IF NOT EXISTS( SELECT TOP 1 1 FROM [dbo].[AppPlatform]
        WHERE [Key] = @PlatformKey
                AND [dbo].[fn_ObjectCanUpdateOrDelete]([State]) =1)
    BEGIN        
        EXEC [dbo].[sp_ThrowException]
            @Name = N'sp_CreateAppVersion',
            @Code = 403,
            @Reason = N'',
            @Message = N'App Platform not found.';
        RETURN;
    END
    ELSE
    BEGIN
        BEGIN TRY
        BEGIN TRANSACTION
            UPDATE [dbo].[AppVersion]
                SET [State] = [dbo].[fn_SetObjectDeleted]([State]),
                    [LastUpdatedStamp] = @NowTime,
                    [LastUpdatedBy] = @OperatorKey
                WHERE [PlatformKey] = @PlatformKey AND [dbo].[fn_ObjectCanUpdateOrDelete]([State]) =1;

            INSERT INTO [dbo].[AppVersion]
               ([Key]
               ,[PlatformKey]
               ,[LatestBuild]
               ,[LatestVersion]
               ,[MinRequiredBuild]
               ,[Note]
               ,[AppServiceStatus]
               ,[CreatedStamp]
               ,[LastUpdatedStamp]
               ,[CreatedBy]
               ,[LastUpdatedBy]
               ,[State])
             VALUES
                   (@Key
                   ,@PlatformKey
                   ,ISNULL(@LatestBuild, 1)
                   ,@LatestVersion
                   ,ISNULL(@MinRequiredBuild, 1)
                   ,@Note
                   ,ISNULL(@AppServiceStatus, 0)
                   ,@NowTime
                   ,@NowTime
                   ,@OperatorKey
                   ,@OperatorKey
                   ,0);
               
            COMMIT TRANSACTION;
        END TRY
        BEGIN CATCH
            ROLLBACK TRANSACTION;
            SET @ErrerMessage = ERROR_MESSAGE();
            SET @ErrorSeverity = ERROR_SEVERITY();
            SET @ErrorState = ERROR_STATE();
            SET @ErrorCode = ERROR_NUMBER();
            RAISERROR(@ErrorCode, @ErrorSeverity,@ErrorState, @ErrerMessage);
        END CATCH
    END
    
    SELECT @Key;
END
GO

