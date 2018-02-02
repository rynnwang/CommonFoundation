IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateBinaryOffsiteDistribution]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateBinaryOffsiteDistribution]
GO

CREATE PROCEDURE [dbo].[sp_CreateOrUpdateBinaryOffsiteDistribution](
    @Identifier UNIQUEIDENTIFIER,
    @Container NVARCHAR(128),
    @HostRegion [NVARCHAR](128),
    @ExpiredStamp DATETIME
)
AS
SET NOCOUNT ON;
DECLARE @UploadCredentialExpiredStamp AS DATETIME;
DECLARE @State AS INT;
DECLARE @Key AS UNIQUEIDENTIFIER;
DECLARE @NowTime AS DATETIME = GETUTCDATE();
BEGIN
    SELECT TOP 1 @Key = [Key], @State = [State], @UploadCredentialExpiredStamp = [UploadCredentialExpiredStamp]
        FROM [dbo].[BinaryOffsiteDistribution]
            WHERE [Identifier] = @Identifier
                AND (@Container IS NULL OR [Container] = @Container)
                AND [HostRegion] = @HostRegion;

    IF @ExpiredStamp IS NULL
        SET @ExpiredStamp = DATEADD(mi, 60, @NowTime);

    IF @State IS NOT NULL AND @State <> 1
    BEGIN
        RETURN;
    END
    ELSE IF @Key IS NULL
    BEGIN
        SET @Key = NEWID();

        INSERT INTO [dbo].[BinaryOffsiteDistribution]
                ([Key]
                ,[Identifier]
                ,[Container]
                ,[HostRegion]
                ,[UploadCredentialExpiredStamp]
                ,[CreatedStamp]
                ,[LastUpdatedStamp]
                ,[State])
            VALUES
                (@Key
                ,@Identifier
                ,@Container
                ,@HostRegion
                ,@ExpiredStamp
                ,@NowTime
                ,@NowTime
                ,1)
    END
    ELSE IF @UploadCredentialExpiredStamp < @NowTime
    BEGIN
        UPDATE [dbo].[BinaryOffsiteDistribution]
            SET 
                [LastUpdatedStamp] = @NowTime,
                [UploadCredentialExpiredStamp] = @ExpiredStamp
            WHERE [Key] = @Key;
    END

    SELECT TOP 1 [Key]
        ,[Identifier]
        ,[Container]
        ,[HostRegion]
        ,[UploadCredentialExpiredStamp]
        ,[CreatedStamp]
        ,[LastUpdatedStamp]
        ,[State]
     FROM [dbo].[BinaryOffsiteDistribution]
     WHERE [Key] = @Key;

END
GO


