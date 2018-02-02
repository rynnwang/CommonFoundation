IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateCentralConfiguration]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateCentralConfiguration];

GO
CREATE PROCEDURE [dbo].[sp_CreateOrUpdateCentralConfiguration](
    @Key [UNIQUEIDENTIFIER],
    @ProductKey [UNIQUEIDENTIFIER],
    @Name [NVARCHAR](256),
    @Configuration [NVARCHAR](MAX),
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
    BEGIN TRY
    BEGIN TRANSACTION

        IF @Key IS NULL
        BEGIN
            SET @Key = NEWID();

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
                ,ISNULL(@Name, '')
                ,ISNULL(@Configuration, '')
                ,@NowTime
                ,@OperatorKey
                ,0);

            
            INSERT INTO [dbo].[CentralConfiguration]
                ([Key]
                ,[SnapshotKey]
                ,[ProductKey]
                ,[CreatedStamp]
                ,[CreatedBy])
            VALUES
                (@Key
                ,@SnapshotKey
                ,@ProductKey
                ,@NowTime
                ,@OperatorKey);
        END
        ELSE IF EXISTS (SELECT TOP 1 1 FROM [dbo].[view_CentralConfiguration] WHERE [Key] = @Key 
            AND [dbo].[fn_ObjectIsWorkable]([State]) = 1)
        BEGIN
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
                ,ISNULL(@Name, '')
                ,ISNULL(@Configuration, '')
                ,@NowTime
                ,@OperatorKey
                ,0);

            UPDATE [dbo].[CentralConfiguration]
                SET [SNapshotKey] = @SnapshotKey
                WHERE [Key] = @Key;
        END
        ELSE
        BEGIN
            EXEC [dbo].[sp_ThrowException]
                @Name = N'sp_CreateOrUpdateCentralConfiguration',
                @Code = 403,
                @Reason = N'[State]',
                @Message = N'Target is not found or cannot modify.';
            RETURN;
        END

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

    SELECT @Key;
END

GO