IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateOrUpdateAppPlatform]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_CreateOrUpdateAppPlatform]
GO

CREATE PROCEDURE [dbo].[sp_CreateOrUpdateAppPlatform](
    @Key UNIQUEIDENTIFIER,
    @Name NVARCHAR(256),
	@PlatformType INT,
	@BundleId [NVARCHAR](256),
	@Url [NVARCHAR](256),
	@MinOSVersion [NVARCHAR](64),
    @Description [NVARCHAR](MAX),
    @OperatorKey UNIQUEIDENTIFIER
)
AS
BEGIN    
    DECLARE @NowTime AS DATETIME = GETUTCDATE();

    IF @Key IS NOT NULL
    BEGIN
        IF EXISTS(SELECT TOP 1 1 FROM [dbo].[AppPlatform]
            WHERE [Key] = @Key
                AND [dbo].[fn_ObjectCanUpdateOrDelete]([State]) =1)
        BEGIN
            UPDATE [dbo].[AppPlatform]
                SET [Name] = ISNULL(@Name, [Name]),
                    [PlatformType] = ISNULL(@PlatformType, [PlatformType]),
                    [BundleId] = ISNULL(@BundleId, [BundleId]),
                    [Url] = ISNULL(@Url, [Url]),
                    [MinOSVersion] = ISNULL(@MinOSVersion, [MinOSVersion]),
                    [Description] = ISNULL(@Description, [Description]),
                    [LastUpdatedStamp] = @NowTime,
                    [LastUpdatedBy] = @OperatorKey
                WHERE [Key] = @Key;
        END
        ELSE
        BEGIN
            EXEC [dbo].[sp_ThrowException]
                @Name = N'sp_CreateAppPlatform',
                @Code = 403,
                @Reason = N'',
                @Message = N'Object state not support that action.';
            RETURN;
        END
    END
    ELSE
    BEGIN
        SET @Key = NEWID();
        INSERT INTO [dbo].[AppPlatform]
               ([Key]
               ,[Name]
               ,[PlatformType]
               ,[BundleId]
               ,[Url]
               ,[MinOSVersion]
               ,[Description]
               ,[CreatedStamp]
               ,[LastUpdatedStamp]
               ,[CreatedBy]
               ,[LastUpdatedBy]
               ,[State])
         VALUES
               (@Key
               ,ISNULL(@Name, CONVERT(NVARCHAR(256), @Key))
               ,ISNULL(@PlatformType, 0)
               ,@BundleId
               ,@Url
               ,@MinOSVersion
               ,@Description
               ,@NowTime
               ,@NowTime
               ,@OperatorKey
               ,@OperatorKey
               ,0);
    END
    
    SELECT @Key;
END
GO

