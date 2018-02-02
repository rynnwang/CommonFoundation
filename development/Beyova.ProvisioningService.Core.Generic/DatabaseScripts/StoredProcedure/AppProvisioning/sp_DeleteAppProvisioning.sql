IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteAppProvisioning]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_DeleteAppProvisioning]
GO

CREATE PROCEDURE [dbo].[sp_DeleteAppProvisioning](
    @Key UNIQUEIDENTIFIER,
    @OperatorKey UNIQUEIDENTIFIER
)
AS
SET NOCOUNT ON;
    DECLARE @NowTime AS DATETIME = GETUTCDATE();
    DECLARE @SnapshotKey AS UNIQUEIDENTIFIER =NEWID();
    DECLARE @LastSnapshotKey AS UNIQUEIDENTIFIER;
    DECLARE @ErrerMessage AS NVARCHAR(MAX);
    DECLARE @ErrorSeverity AS INT;
    DECLARE @ErrorState AS INT;
    DECLARE @ErrorCode AS INT;

BEGIN    
    SELECT TOP 1 @LastSnapshotKey = [Key] FROM [dbo].[view_AppProvisioning]
        WHERE [Key] = @Key
            AND [dbo].[fn_ObjectCanUpdateOrDelete]([State]) =1;
    IF @LastSnapshotKey IS NOT NULL
    BEGIN
        BEGIN TRY
        BEGIN TRANSACTION
            UPDATE [dbo].[AppProvisioning]
                SET [SnapshotKey] = @SnapshotKey
                WHERE [Key] = @Key;

            INSERT INTO [dbo].[AppProvisioningSnapshot]
               ([Key]
               ,[ProvisioningKey]
               ,[Content]
               ,[CreatedStamp]
               ,[CreatedBy]
               ,[State])
             SELECT TOP 1
                   @SnapshotKey
                   ,[ProvisioningKey]
                   ,[Content]
                   ,@NowTime
                   ,@OperatorKey
                   ,[dbo].[fn_SetObjectDelete]([State])
                WHERE [Key] = @LastSnapshotKey;
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
    ELSE
    BEGIN
        EXEC [dbo].[sp_ThrowException]
            @Name = N'sp_DeleteAppProvisioning',
            @Code = 403,
            @Reason = N'',
            @Message = N'Object state not support that action.';
        RETURN;
    END
END
GO

