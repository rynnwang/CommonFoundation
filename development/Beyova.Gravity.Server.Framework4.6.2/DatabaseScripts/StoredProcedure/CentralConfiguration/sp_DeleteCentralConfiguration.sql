IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_DeleteCentralConfiguration]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_DeleteCentralConfiguration];

GO
CREATE PROCEDURE [dbo].[sp_DeleteCentralConfiguration](
    @Key [UNIQUEIDENTIFIER],
    @OperatorKey [UNIQUEIDENTIFIER]
)
AS
DECLARE @ErrerMessage AS NVARCHAR(MAX);
DECLARE @ErrorSeverity AS INT;
DECLARE @ErrorState AS INT;
DECLARE @ErrorCode AS INT;
DECLARE @NowTime AS DATETIME = GETUTCDATE();
DECLARE @SnapshotKey AS UNIQUEIDENTIFIER = NEWID();
BEGIN
    IF @Key IS NOT NULL 
        AND @OperatorKey IS NOT NULL
        AND EXISTS (SELECT TOP 1 1 FROM [dbo].[view_CentralConfigurationSnapshot]
            WHERE [Key] = @Key
                AND [dbo].[fn_ObjectCanUpdateOrDelete]([State]) = 1)
    BEGIN
        BEGIN TRY
        BEGIN TRANSACTION
            INSERT INTO [dbo].[CentralConfigurationSnapshot]
                ([Key]
                ,[ConfigurationKey]
                ,[Name]
                ,[Configuration]
                ,[CreatedStamp]
                ,[CreatedBy]
                ,[State])
            VALUES
                (@SnapshotKey
                ,@Key
                ,''
                ,''
                ,@NowTime
                ,@OperatorKey
                ,1);

            UPDATE [dbo].[CentralConfiguration]
                SET [SNapshotKey] = @SnapshotKey,
                    [IsDeleted] = 1
                WHERE [Key] = @Key;
        COMMIT TRANSACTION
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
            @Name = N'sp_DeleteCentralConfiguration',
            @Code = 403,
            @Reason = N'[State]',
            @Message = N'Target is not found or cannot modify.';
        RETURN;
    END
END

GO