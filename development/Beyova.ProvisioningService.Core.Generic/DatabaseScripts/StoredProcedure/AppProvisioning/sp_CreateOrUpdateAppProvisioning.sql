IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateAppProvisioning]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateAppProvisioning]
GO

CREATE PROCEDURE [dbo].[sp_CreateOrUpdateAppProvisioning](
    @Name NVARCHAR(256),
    @PlatformKey UNIQUEIDENTIFIER,
    @Content [NVARCHAR](MAX),
    @OperatorKey UNIQUEIDENTIFIER
)
AS
BEGIN    
    DECLARE @NowTime AS DATETIME = GETUTCDATE();
    DECLARE @Key AS UNIQUEIDENTIFIER;
    DECLARE @SnapshotKey AS UNIQUEIDENTIFIER = NEWID();
    DECLARE @ErrerMessage AS NVARCHAR(MAX);
    DECLARE @ErrorSeverity AS INT;
    DECLARE @ErrorState AS INT;
    DECLARE @ErrorCode AS INT;

    SET @Name = LTRIM(RTRIM(ISNULL(@Name, '')));

    IF @PlatformKey IS NOT NULL
    BEGIN
        SELECT Top 1 @Key = [Key]
            FROM [dbo].[view_AppProvisioning] 
            WHERE [PlatformKey] = @PlatformKey
                AND [Name] = @Name
                AND [dbo].[fn_ObjectIsWorkable]([State]) = 1;

        BEGIN TRY
        BEGIN TRANSACTION
            IF (@Key IS NULL)
            BEGIN
                SET @Key = NEWID();

                INSERT INTO [dbo].[AppProvisioning]
                    ([Key]
                    ,[SnapshotKey]
                    ,[PlatformKey]
                    ,[Name]
                    ,[CreatedStamp]
                    ,[CreatedBy])
                VALUES
                    (@Key
                    ,@SnapshotKey
                    ,@PlatformKey
                    ,@Name
                    ,@NowTime
                    ,@OperatorKey);
            END
            ELSE
            BEGIN
                UPDATE [dbo].[AppProvisioning]
                    SET [SnapshotKey] = @SnapshotKey
                    WHERE [Key] = @Key;
            END

            INSERT INTO [dbo].[AppProvisioningSnapshot]
                ([Key]
                ,[ProvisioningKey]
                ,[Content]
                ,[CreatedStamp]
                ,[CreatedBy]
                ,[State])
            VALUES
                (@SnapshotKey
                ,@Key
                ,@Content
                ,@NowTime
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

